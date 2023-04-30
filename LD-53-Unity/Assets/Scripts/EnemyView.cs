using System.Collections.Generic;
using UnityEngine;

public class EnemyView : ObjectGridElement
{
    [SerializeField] private int _damage;
    [SerializeField] private List<Vector2Int> _agroDeltaPoints;

    [SerializeField] private EncounterReward _encounterReward;
    
    public int Damage => _damage;
    public List<Vector2Int> AgroDeltaPoints => _agroDeltaPoints;

    public EncounterReward EncounterReward => _encounterReward;
}