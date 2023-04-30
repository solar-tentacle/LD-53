using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyService : IService, IInject, IStart
{
    private GridService _gridService;
    private List<EnemyView> _enemies = new();
    private List<GroundGridElement> _agroGround = new();
    private PlayerView _playerView;
    private UnitService _unitService;

    void IInject.Inject()
    {
        _gridService = Services.Get<GridService>();
        _playerView = Services.Get<PlayerService>().PlayerView;
        _unitService = Services.Get<UnitService>();
    }


    void IStart.GameStart()
    {
        _enemies = _gridService.GetEnemies();

        foreach (EnemyView enemy in _enemies)
        {
            Vector2Int pos = _gridService.GetObjectPosition(enemy);

            foreach (Vector2Int delta in enemy.AgroDeltaPoints)
            {
                _gridService.TryAddGroundElement(_agroGround, pos + delta);
            }
        }

        
    }

    public IEnumerator EnableHighlight()
    {
        foreach (GroundGridElement ground in _agroGround)
        {
            yield return ground.EnableHighlight(HighlightType.Agro);
        }
    }
    
    public IEnumerator DisableHighlight()
    {
        foreach (GroundGridElement ground in _agroGround)
        {
            yield return ground.DisableHighlight(HighlightType.Agro);
        }
    }

    public bool IsAgroGround(Vector2Int pos)
    {
        foreach (GroundGridElement ground in _agroGround)
        {
            if (_gridService.GetGroundPosition(ground) == pos)
            {
                return true;
            }
        }

        return false;
    }

    public IEnumerator BattleFlow()
    {
        EnemyView enemy = FindNearestEnemy();

        yield return DisableHighlight();
        
        _unitService.ChangeUnitHealth(_playerView, -enemy.Damage);
    }

    public EnemyView FindNearestEnemy()
    {
        EnemyView nearestEnemy = null;
        float minDistance = float.MaxValue;

        foreach (EnemyView enemy in _enemies)
        {
            float distance = Vector3.Distance(_playerView.transform.position, enemy.transform.position);
            if (minDistance < Vector3.Distance(_playerView.transform.position, enemy.transform.position))
            {
                minDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }
}