using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterService : IService, IInject, IStart
{
    private GridService _gridService;
    private Dictionary<Vector2Int, ObjectGridElement> _encounterPositions;

    public void Inject()
    {
        _gridService = Services.Get<GridService>();
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

    public IEnumerator Flow(EncounterGridElement encounter)
    {
        Debug.Log("Ya huesos");
        yield break;
        //showWindow;
        // yield return waitForClick;
        // yield return animationNagradi;
        // yield return closeWindow;
    }
}