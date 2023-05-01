using System.Collections.Generic;
using UnityEngine;

public class EnemyView : ObjectGridElement
{
    private static readonly int MoveTrigger = Animator.StringToHash("Move");
    private static readonly int EndMoveTrigger = Animator.StringToHash("EndMove");
    private static readonly int AttackTrigger = Animator.StringToHash("Attack");

    [SerializeField] private Animator _animator;
    [SerializeField] private List<Vector2Int> _agroDeltaPoints;

    [SerializeField] private EncounterReward _encounterReward;
    
    public List<Vector2Int> AgroDeltaPoints => _agroDeltaPoints;

    public EncounterReward EncounterReward => _encounterReward;

    public void StartMove() => _animator.SetTrigger(MoveTrigger);
    public void EndMove() => _animator.SetTrigger(EndMoveTrigger);
    public void Attack() => _animator.SetTrigger(AttackTrigger);
}