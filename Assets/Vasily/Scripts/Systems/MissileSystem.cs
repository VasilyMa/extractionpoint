using System.Collections.Generic;

using UnityEngine;

public class MissileSystem : SourceSystem
{
    public static MissileSystem Instance { get; private set; }
    protected List<SourceMissile> _missiles;

    public MissileSystem(BattleState state) : base(state)
    {
        _missiles = new List<SourceMissile>();
        Instance = this;
    }

    public override void Dispose()
    {

    }

    public override void AfterInit()
    {
        Debug.Log($"After init {this}");
    }

    public override void Run()
    {
        for (int i = 0; i < _missiles.Count; i++)
        {
            if (_missiles[i].Run()) continue;

            _missiles.RemoveAt(i);
        }
    }
    
    public void Add(SourceMissile sourceMissile)
    {
        _missiles.Add(sourceMissile);
    }
    
    public override void FixedRun()
    {
    }

    public override void Init()
    {

    }
}
