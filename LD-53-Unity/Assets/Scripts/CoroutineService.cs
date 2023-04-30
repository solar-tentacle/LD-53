using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineService : Service
{
    public IEnumerator ExecuteAction(CardAction action)
    {
        yield return action.Execute();
    }
}