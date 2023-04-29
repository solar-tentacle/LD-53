using System;
using UnityEngine;

public class SpawnerService : Service, IStart
{
    private ObjectToSpawn _object;
    private RaycastHit[] _buffer;
    private Camera _cam;

    public void GameStart()
    {
        _object = Services.Get<ObjectToSpawn>();
        // CardView.OnThrowed += SpawnObject;
        _cam = Camera.main;
    }

    private void SpawnObject()
    {
        Debug.Log("Card spawned [CardSpawner]");
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
            Instantiate(_object, hit.point, Quaternion.identity);
    }

    private void OnDestroy()
    {
        // CardView.OnThrowed -= SpawnObject;
    }
}