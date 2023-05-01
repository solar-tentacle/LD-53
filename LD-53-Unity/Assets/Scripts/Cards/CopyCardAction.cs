using System;
using System.Collections;

[Serializable]
public class CopyCardAction : CardAction
{
    private CardHandService _cardHandService;

    public override void Init()
    {
        _cardHandService = Services.Get<CardHandService>();
    }

    public override IEnumerator Select()
    {
        yield return null;
    }

    public override IEnumerator Deselect()
    {
        yield return null;
    }

    public override bool CanExecute()
    {
        return true;
    }

    public override IEnumerator Execute()
    {
        yield return _cardHandService.CopyCardFlow();
    }
}