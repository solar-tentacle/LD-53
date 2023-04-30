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

    void IInject.Inject()
    {
        _cardHandService = Services.Get<CardHandService>();
        _coroutineService = Services.Get<CoroutineService>();
        _cardDeckService = Services.Get<CardDeckService>();
        _enemyService = Services.Get<EnemyService>();
        _playerView = Services.Get<PlayerService>().PlayerView;
        _gridService = Services.Get<GridService>();
        _uiBattle = Services.Get<UIService>().UICanvas.HUD.BattleInProgress;
    }


    void IStart.GameStart()
    {
        _coroutineService.StartCoroutine(FlowCoroutine());
    }

    private IEnumerator FlowCoroutine()
    {
        while (true)
        {
            yield return _enemyService.EnableHighlight();
            
            yield return _cardHandService.SelectCardFlow();
            Card card = _cardHandService.SelectedCard;
            CardAction action = card.Config.Action;
            
            yield return action.Select();
            
            while (true)
            {
                if (Input.GetMouseButtonDown(0) && action.CanExecute())
                {
                    yield return action.Deselect();
                    yield return action.Execute();
                    break;
                }

                yield return null;
            }
            
            yield return _cardHandService.HideCardFlow();
            _cardDeckService.TryAddCardFromCurrentDeck(card.Config.CardType);

            bool inAgro = IsPlayerInAgroGround();
            if (inAgro && _isBattle == false)
            {
                _isBattle = true;
                _uiBattle.SetActive(true);
                continue;
            }

            if (inAgro == false && _isBattle)
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

    private bool IsPlayerInAgroGround() => _enemyService.IsAgroGround(_gridService.GetObjectPosition(_playerView));
}