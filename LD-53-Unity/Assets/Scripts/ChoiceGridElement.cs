using UnityEngine;

public class ChoiceGridElement : ObjectGridElement
{
    [SerializeField] private EncounterData _encounterData;
    
    public EncounterData EncounterData => _encounterData;
}