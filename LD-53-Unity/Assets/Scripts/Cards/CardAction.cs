using System;
using System.Collections;

[Serializable]
public abstract class CardAction
{
    public abstract IEnumerator Execute();
}