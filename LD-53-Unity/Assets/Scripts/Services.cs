using System;
using System.Collections.Generic;
using UnityEngine;

public interface IService
{
}

public interface IUpdate
{
    void GameUpdate(float delta);
}

public interface ILateUpdate
{
    void GameLateUpdate(float delta);
}

public interface IStart
{
    void GameStart();
}

public interface IInject
{
    void Inject();
}

public abstract class Service : MonoBehaviour, IService
{
}

public class Services : MonoBehaviour
{
    private static Dictionary<Type, IService> _services = new();
    private static List<IUpdate> _updates = new();
    private static List<ILateUpdate> _lateUpdates = new();
    private static List<IStart> _starts = new();
    private static List<IInject> _injects = new();

    private void Awake()
    {
        CreateNonMonoServices();
        Service[] services = FindObjectsOfType<Service>();
        foreach (Service service in services)
        {
            Register(service);
        }

        foreach (IInject inject in _injects)
        {
            inject.Inject();
        }
    }

    private void CreateNonMonoServices()
    {
        Register(new GridService());
        Register(new PlayerService());
        Register(new DragAndDropService());
        Register(new CardDeckService());
        Register(new CardHandService());
        Register(new UnitService());
    }
    
    private void Register(IService service)
    {
        _services.Add(service.GetType(), service);
        
        if (service is IUpdate update)
        {
            _updates.Add(update);
        }

        if (service is ILateUpdate lateUpdate)
        {
            _lateUpdates.Add(lateUpdate);
        }

        if (service is IStart start)
        {
            _starts.Add(start);
        }

        if (service is IInject inject)
        {
            _injects.Add(inject);
        }
    }

    private void Start()
    {
        foreach (IStart start in _starts)
        {
            start.GameStart();
        }
    }

    private void Update()
    {
        foreach (IUpdate update in _updates)
        {
            update.GameUpdate(Time.deltaTime);
        }
    }

    private void LateUpdate()
    {
        foreach (ILateUpdate update in _lateUpdates)
        {
            update.GameLateUpdate(Time.deltaTime);
        }
    }

    private void OnDestroy()
    {
        _services.Clear();
        _updates.Clear();
        _lateUpdates.Clear();
        _starts.Clear();
        _injects.Clear();
    }

    public static T Get<T>() where T : IService => (T) _services[typeof(T)];
}