using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PortalService : IService, IInject, IStart
{
    private PlayerMovement _movement;
    private GridService _gridService;
    private PlayerService _playerService;
    private Dictionary<Vector2Int,ObjectGridElement> _portals;

    public PlayerView PlayerView { get; private set; }

    void IInject.Inject()
    {
        _gridService = Services.Get<GridService>();
        _playerService = Services.Get<PlayerService>();
    }

    void IStart.GameStart()
    {
        _portals = _gridService.GetObjectsPositions(ObjectType.Portal);
    }
    
    public bool TryGetPortal(Vector2Int playerPos, out PortalGridElement result)
    {
        result = null;
        if (_portals.TryGetValue(playerPos, out var element))
        {
            result = element as PortalGridElement;
            return true;
        }

        return false;
    }

    public IEnumerator Teleport(Vector2Int playerPosition)
    {
        var (newPosition, portalPosition) = GetNearestPortalPosition(playerPosition);
        yield return _playerService.Move(portalPosition, 0, true);
        yield return _playerService.Move(newPosition);
    }

    private (Vector2Int, Vector2Int) GetNearestPortalPosition(Vector2Int playerPosition)
    {
        float minDistance = float.MaxValue;
        Vector2Int? resultPosition = null;
        Vector2Int? portalPosition = null;
        foreach (var portal in _portals)
        {
            if (portal.Key == playerPosition)
            {
                continue;
            }

            var distance = Vector2Int.Distance(portal.Key, playerPosition);

            var portalData = (portal.Value as PortalGridElement).Data;
            var portalOutPosition = portal.Key + portalData.Direction;

            if ((!resultPosition.HasValue || distance < minDistance) && !_portals.ContainsKey(portalOutPosition) &&
                _gridService.TryAddGroundElement(new List<GroundGridElement>(), portalOutPosition, ObjectType.Portal))
            {
                minDistance = distance;
                resultPosition = portalOutPosition;
                portalPosition = portal.Key;
            }
        }

        if (!resultPosition.HasValue)
        {
            var originalPortalData = (_portals[playerPosition] as PortalGridElement).Data;
            resultPosition = playerPosition + originalPortalData.Direction;
        }

        return (resultPosition.Value, portalPosition.Value);
    }
}