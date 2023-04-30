using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterService : IService, IInject, IStart
{
    private GridService _gridService;
    private Dictionary<Vector2Int, ObjectGridElement> _encounterPositions;
    private UIEncounterWindow _uiEncounterWindow;
    private UIService _uiService;

    public void Inject()
    {
        _gridService = Services.Get<GridService>();
        _uiService = Services.Get<UIService>();
        _uiEncounterWindow = _uiService.UICanvas.UIEncounterWindow;
    }

    public void GameStart()
    {
        _encounterPositions = _gridService.GetObjectsPositions(ObjectType.Encounter);
    }

    public bool TryGetEncounter(Vector2Int playerPos, out EncounterGridElement encounter)
    {
        encounter = null;
        if (_encounterPositions.TryGetValue(playerPos, out var element))
        {
            encounter = element as EncounterGridElement;
            return true;
        }

        return false;
    }

    public IEnumerator Flow(EncounterGridElement encounter, Vector2Int encounterPosition)
    {
        encounter.gameObject.SetActive(false);
        _encounterPositions.Remove(encounterPosition);
        _uiEncounterWindow.SetContent(encounter.EncounterData);
        _uiEncounterWindow.ShowEncounterWindow();

        yield return new WaitUntil(() => _uiEncounterWindow.Ended);
        if (_uiEncounterWindow.CurrentAnswerData.Reward != null)
        {
            yield return _uiEncounterWindow.CurrentAnswerData.Reward.GiveReward();

        }
    }
}