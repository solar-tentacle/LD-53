using System;
using UnityEngine;

[Serializable]
public class EncounterData
{
    public Sprite Icon;
    public string Question;
    public AnswerEncounterData AnswerData1;
    public AnswerEncounterData AnswerData2;
}

[Serializable]
public class AnswerEncounterData
{
    public string AnswerText;
    public string ResultText;
    public EncounterReward Reward;
}