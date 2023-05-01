using System.Collections.Generic;
using UnityEngine;

public class EnemyView : ObjectGridElement
{
    [SerializeField] private List<Vector2Int> _agroDeltaPoints;

    public List<Vector2Int> AgroDeltaPoints => _agroDeltaPoints;

    public EncounterReward EncounterReward;
}