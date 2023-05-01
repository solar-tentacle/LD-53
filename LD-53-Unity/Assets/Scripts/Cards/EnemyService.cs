using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
            EnemyContext ctx = CreateContext(enemy);
            _enemyContexts.Add(enemy, ctx);
        }
    }

    private EnemyContext CreateContext(EnemyView view)
    {
        EnemyContext ctx = new();
        ctx.AgroGround = new List<GroundGridElement>();
        Vector2Int pos = _gridService.GetObjectPosition(view);

        foreach (Vector2Int delta in view.AgroDeltaPoints)
        {
            _gridService.TryAddGroundElement(ctx.AgroGround, pos + delta);
        }

        return ctx;
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

    public void RemoveEnemy(EnemyView view)
    {
        foreach (GroundGridElement ground in _enemyContexts[view].AgroGround)
        {
            ground.StartCoroutine(ground.DisableHighlight(HighlightType.Agro));
        }

        _enemyContexts.Remove(view);
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

    public bool FarFromEnemies(Vector2Int pos)
    {
        foreach (EnemyView view in _enemyContexts.Keys)
        {
            Vector2Int enemyPos = _gridService.GetObjectPosition(view);
            float distance = Vector2Int.Distance(pos, enemyPos);
            if (distance < 2.85f) return false;
        }

        return true;
    }

    public IEnumerator BattleFlow()
    {
        EnemyView enemy = FindNearestEnemy();

        if (enemy is null)
        {
            yield break;
        }
        
        yield return DisableHighlight();
        
        Vector2Int enemyPos = _gridService.GetObjectPosition(enemy);
        Vector2Int playerPos = _gridService.GetObjectPosition(_playerView);
        float distance = Vector2Int.Distance(enemyPos, playerPos);
        if (distance > 1.5)
        {
            yield return TryMove(enemy, enemyPos, playerPos);
        }
        else
        {
            yield return _unitService.AttackObject(enemy, _playerView);
        }
    }

    private IEnumerator TryMove(EnemyView view, Vector2Int enemyPos, Vector2Int playerPos)
    {
        if (enemyPos.x > playerPos.x && _gridService.IsMovablePoint(enemyPos + Vector2Int.left))
        {
            yield return Move(view, enemyPos + Vector2Int.left);
        }
        else if (enemyPos.x < playerPos.x && _gridService.IsMovablePoint(enemyPos + Vector2Int.right))
        {
            yield return Move(view, enemyPos + Vector2Int.right);
        }
        else if (enemyPos.y > playerPos.y && _gridService.IsMovablePoint(enemyPos + Vector2Int.down))
        {
            yield return Move(view, enemyPos + Vector2Int.down);
        }
        else if (enemyPos.y < playerPos.y && _gridService.IsMovablePoint(enemyPos + Vector2Int.up))
        {
            yield return Move(view, enemyPos + Vector2Int.up);
        }
    }

    private EnemyView FindNearestEnemy()
    {
        EnemyView nearestEnemy = null;
        float minDistance = float.MaxValue;

        foreach (EnemyView enemy in _enemyContexts.Keys)
        {
            if (_unitService.IsStunned(enemy))
            {
                continue;
            }
            
            float distance = Vector3.Distance(_playerView.transform.position, enemy.transform.position);
            if (minDistance > Vector3.Distance(_playerView.transform.position, enemy.transform.position))
            {
                minDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    private IEnumerator Move(EnemyView view, Vector2Int pos)
    {
        _gridService.Move(view, pos);
        Vector3 newPosition = _gridService.GetWorldPoint(pos);
        newPosition.y = view.transform.position.y;

        yield return view.transform.DOMove(newPosition, 0.5f).WaitForCompletion();
        _enemyContexts[view] = CreateContext(view);
    }
}