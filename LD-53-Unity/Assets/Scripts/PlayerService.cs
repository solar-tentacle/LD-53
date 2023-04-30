using System.Collections;
using UnityEngine;

public class PlayerService : IService, IInject, IStart, IUpdate
{
    private PlayerView _playerView;
    private PlayerMovement _movement;
    private GridService _gridService;

    void IInject.Inject()
    {
        _gridService = Services.Get<GridService>();
    }

    void IStart.GameStart()
    {
        _playerView = _gridService.GetPlayerView();

        _movement = new PlayerMovement(_playerView);
        _movement.Init();
    }

    void IUpdate.GameUpdate(float delta)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Plane plane = new(Vector3.up, 0);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out float distance))
            {
                Vector3 worldPosition = ray.GetPoint(distance);

                Vector2Int pos = _gridService.GetMatrixPosition(worldPosition);
                Move(pos);
            }
        }
    }

    public void Move(Vector2Int movePosition)
    {
        _playerView.StartCoroutine(_movement.Move(movePosition));
    }
}