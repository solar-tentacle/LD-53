using System;
using System.Collections;
using UnityEngine;

public class FlowService : IService, IInject, IStart
{
    private CardHandService _cardHandService;
    private CoroutineService _coroutineService;
    private CardDeckService _cardDeckService;
    private EnemyService _enemyService;
    private bool _isBattle;
    private PlayerView _playerView;
    private GridService _gridService;
    private GameObject _uiBattle;
    private GameFlowService _gameFlowService;
    private EncounterService _encounterService;
    private UIService _uiService;
    private Vector2Int _endLevelPosition;
    private AssetsCollection _assetsCollection;
    private bool _usedMovementAction;
    private PortalService _portalService;
    private ChestService _chestService;
    private bool _cancelSelection;

    void IInject.Inject()
    {
        _cardHandService = Services.Get<CardHandService>();
        _coroutineService = Services.Get<CoroutineService>();
        _cardDeckService = Services.Get<CardDeckService>();
        _enemyService = Services.Get<EnemyService>();
        _gridService = Services.Get<GridService>();
        _uiBattle = Services.Get<UIService>().UICanvas.HUD.BattleInProgress;
        _gameFlowService = Services.Get<GameFlowService>();
        _encounterService = Services.Get<EncounterService>();
        _portalService = Services.Get<PortalService>();
        _uiService = Services.Get<UIService>();
        _assetsCollection = Services.Get<AssetsCollection>();
        _chestService = Services.Get<ChestService>();
    }

    void IStart.GameStart()
    {
        _coroutineService.StartCoroutine(FlowCoroutine());
        _playerView = Services.Get<PlayerService>().PlayerView;
        _endLevelPosition = _gridService.GetObjectPosition(_gridService.GetEndLevelView());
    }

    private IEnumerator FlowCoroutine()
    {
        while (true)
        {
            _usedMovementAction = false;

            yield return _enemyService.EnableHighlight();

            yield return _cardHandService.SelectCardFlow(_isBattle);

            Card card = _cardHandService.SelectedCard;
            CardAction action = card.Config.Action;
            yield return HandleCardAction(_cardHandService.SelectedCardView, action);

            if (_cancelSelection)
            {
                yield return _cardHandService.CancelFlow();
                continue;
            }

            if (_cardHandService.SelectedCard != null)
            {
                yield return _cardHandService.HideCardFlow();
            }

            yield return TryAddCard(card);

            var playerPos = _gridService.GetObjectPosition(_playerView);

            if (_usedMovementAction)
            {
                if (_encounterService.TryGetEncounter(playerPos, out var encounter))
                {
                    yield return _encounterService.Flow(encounter, playerPos);
                }

                if (_chestService.TryGetChest(playerPos, out var chest))
                {
                    yield return _chestService.Flow(chest, playerPos);
                }

                if (playerPos == _endLevelPosition)
                {
                    _uiService.UICanvas.UIWinWindow.Show();
                    yield return _uiService.UICanvas.UIWinWindow.WaitForClose();
                    yield return _gameFlowService.CompleteLevel();
                    yield break;
                }

                if (_portalService.TryGetPortal(playerPos, out var portal))
                {
                    yield return _portalService.Teleport(playerPos);
                }
            }

            if (!_cardHandService.Has(CardType.Movement))
            {
                _gameFlowService.LoseGame(_assetsCollection.GameConfig.EndMoveCardsLoseReasonText);
                yield break;
            }

            bool inAgro = IsPlayerInAgro();
            if (inAgro && _isBattle == false)
            {
                _isBattle = true;
                _uiBattle.SetActive(true);
                continue;
            }

            bool farFromEnemies = FarFromEnemies();
            if (farFromEnemies && _isBattle)
            {
                _isBattle = false;
                _uiBattle.SetActive(false);
            }

            if (_isBattle)
            {
                yield return _enemyService.BattleFlow();
            }
        }
    }

    private bool IsPlayerInAgro() => _enemyService.IsAgroGround(_gridService.GetObjectPosition(_playerView));
    private bool FarFromEnemies() => _enemyService.FarFromEnemies(_gridService.GetObjectPosition(_playerView));

    private IEnumerator TryAddCard(Card card)
    {
        CardType type = card.Config.CardType;

        switch (type)
        {
            case CardType.Movement:
            {
                yield return _cardDeckService.TryAddCardFromCurrentDeck(CardType.Movement);

                if (card.Config.Action is MovementCard {DirectionsCount: 16})
                {
                    yield return _cardDeckService.TryAddCardFromCurrentDeck(CardType.Movement);
                }

                break;
            }
            case CardType.Action when _isBattle:
                yield return _cardDeckService.TryAddCardFromCurrentDeck(CardType.Action);
                break;
        }
    }

    private IEnumerator HandleCardAction(CardView view, CardAction action)
    {
        if (action is GetCardsFromHandAction or CopyCardAction)
        {
            yield return action.Execute();
            yield break;
        }

        _cancelSelection = false;

        yield return action.Select();

        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(view.Container, Input.mousePosition) ||
                    RectTransformUtility.RectangleContainsScreenPoint(_cardHandService.DarkRect, Input.mousePosition))
                {
                    _cancelSelection = true;
                    yield return action.Deselect();
                    yield break;
                }

                if (action.CanExecute())
                {
                    yield return action.Deselect();
                    yield return action.Execute();
                    if (action is MovementCard)
                    {
                        _usedMovementAction = true;
                    }

                    break;
                }
            }

            yield return null;
        }
    }
}