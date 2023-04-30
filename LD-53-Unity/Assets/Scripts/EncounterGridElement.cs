using UnityEngine;

public class EncounterGridElement : ObjectGridElement
{
    [SerializeField] private EncounterData _encounterData;
    
    public EncounterData EncounterData => _encounterData;
}