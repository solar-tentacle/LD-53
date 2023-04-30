using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContext
{
    public List<GroundGridElement> AgroGround;
}

public class EnemyService : IService, IInject, IStart
{
    private GridService _gridService;
    private Dictionary<EnemyView, EnemyContext> _enemyContexts = new();
    private PlayerView _playerView;
    private UnitService _unitService;

    void IInject.Inject()
    {
        _gridService = Services.Get<GridService>();
        _unitService = Services.Get<UnitService>();
    }

    void IStart.GameStart()
    {
        _playerView = Services.Get<PlayerService>().PlayerView;
        List<EnemyView> enemies = _gridService.GetEnemies();
        

        foreach (EnemyView enemy in enemies)
        {
            EnemyContext ctx = new();
            ctx.AgroGround = new List<GroundGridElement>();
            Vector2Int pos = _gridService.GetObjectPosition(enemy);

            foreach (Vector2Int delta in enemy.AgroDeltaPoints)
            {
                _gridService.TryAddGroundElement(ctx.AgroGround, pos + delta);
            }
            
            _enemyContexts.Add(enemy, ctx);
        }
    }

    public IEnumerator EnableHighlight()
    {
        foreach ((EnemyView view, EnemyContext ctx) in _enemyContexts)
        {
            foreach (GroundGridElement ground in ctx.AgroGround)
            {
                yield return ground.EnableHighlight(HighlightType.Agro);
            }
        }
    }
    
    public IEnumerator DisableHighlight()
    {
        foreach ((EnemyView view, EnemyContext ctx) in _enemyContexts)
        {
            foreach (GroundGridElement ground in ctx.AgroGround)
            {
                yield return ground.DisableHighlight(HighlightType.Agro);
            }
        }
    }

    public bool IsAgroGround(Vector2Int pos)
    {
        foreach ((EnemyView view, EnemyContext ctx) in _enemyContexts)
        {
            foreach (GroundGridElement ground in ctx.AgroGround)
            {
                if (_gridService.GetGroundPosition(ground) == pos)
                {
                    return true;
                }
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

    private EnemyView FindNearestEnemy()
    {
        EnemyView nearestEnemy = null;
        float minDistance = float.MaxValue;

        foreach (EnemyView enemy in _enemyContexts.Keys)
        {
            float distance = Vector3.Distance(_playerView.transform.position, enemy.transform.position);
            if (minDistance > Vector3.Distance(_playerView.transform.position, enemy.transform.position))
            {
                minDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    public void RemoveEnemy(EnemyView view)
    {
        foreach (GroundGridElement ground in _enemyContexts[view].AgroGround)
        {
            ground.StartCoroutine(ground.DisableHighlight(HighlightType.Agro));
        }

        _enemyContexts.Remove(view);
    }
}