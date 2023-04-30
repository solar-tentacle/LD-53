using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineService : Service
{
    public IEnumerator ExecuteActionCrt(CardAction action)
    {
        yield return action.Execute();
    }

    public void ExecuteAction(CardAction action)
    {
        StartCoroutine(ExecuteActionCrt(action));
    }
}