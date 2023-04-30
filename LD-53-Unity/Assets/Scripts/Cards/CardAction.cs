using System;
using System.Collections;

[Serializable]
public abstract class CardAction
{
    public abstract void Init();
    public abstract IEnumerator Select();
    public abstract IEnumerator Deselect();
    public abstract bool CanExecute();
    public abstract IEnumerator Execute();
}