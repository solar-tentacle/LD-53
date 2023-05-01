using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class ChangeHealthAction : CardAction
{
    [SerializeField] private float _changeValue;

    private UnitService _unitService;
    private PlayerService _playerService;
    
    public override void Init()
    {
        _unitService = Services.Get<UnitService>();
        _playerService = Services.Get<PlayerService>();
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
        yield return _unitService.ChangeUnitHealth(_playerService.PlayerView, (int)_changeValue);
    }
}