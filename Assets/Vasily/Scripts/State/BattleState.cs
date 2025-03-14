using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class BattleState : State
{
    public static new BattleState Instance
    {
        get
        {
            return (BattleState)State.Instance;
        }
    }
    [SerializeReference] protected List<SourceSystem> _systems = new List<SourceSystem>();

    public virtual void AfterInit()
    {
        Debug.Log($"After init {this}");

        for (int i = 0; i < _systems.Count; i++) _systems[i].AfterInit();
    }

    public virtual void Run()
    {
        for (int i = 0; i < _systems.Count; i++) _systems[i].Run();
    }
    public virtual void FixedRun()
    {
        for (int i = 0; i < _systems.Count; i++) _systems[i].FixedRun();
    }

    public virtual T GetSystems<T>() where T : SourceSystem
    {
        T existingSystem = _systems.OfType<T>().FirstOrDefault();

        if (existingSystem != null) return existingSystem;

        T newSystem = Activator.CreateInstance<T>();

        _systems.Add(newSystem);

        return newSystem;
    }

    public override void Dispose()
    {
        _systems.ForEach(s => s.Dispose());
        _canvases.ForEach(c => c.Dispose());
        _pool.Dispose();
    }
}
