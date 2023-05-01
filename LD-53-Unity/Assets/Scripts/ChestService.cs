using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestService : IService, IInject, IStart
{
    private GridService _gridService;
    private Dictionary<Vector2Int, ObjectGridElement> _chestPositions;

    public void Inject()
    {
        _gridService = Services.Get<GridService>();
    }

    public void GameStart()
    {
        _chestPositions = _gridService.GetObjectsPositions(ObjectType.Chest);
    }

    public bool TryGetChest(Vector2Int playerPos, out ChestGridElement chest)
    {
        chest = null;
        if (_chestPositions.TryGetValue(playerPos, out var element))
        {
            chest = element as ChestGridElement;
            return true;
        }

        return false;
    }

    public IEnumerator Flow(ChestGridElement chest, Vector2Int chestPosition)
    {
        chest.gameObject.SetActive(false);
        _chestPositions.Remove(chestPosition);
        yield return chest.Reward.GiveReward();
    }
}