using System.Collections;
using UnityEngine;

public class PlayerService : IService, IInject, IStart, IUpdate
{
    private PlayerMovement _movement;
    private GridService _gridService;

    public PlayerView PlayerView { get; private set; }

    void IInject.Inject()
    {
        _gridService = Services.Get<GridService>();
    }

    void IStart.GameStart()
    {
        PlayerView = _gridService.GetPlayerView();

        _movement = new PlayerMovement(PlayerView);
        _movement.Init();
    }


    void IUpdate.GameUpdate(float delta)
    {

    }

    public IEnumerator Move(Vector2Int movePosition, float? duration = null, bool teleport = false)
    {
        yield return _movement.Move(movePosition, duration, teleport);
    }
}