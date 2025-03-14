using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXSystem : SourceSystem
{
    public static VFXSystem Instance { get; private set; }
    protected List<SourceParticle> _particles;

    public VFXSystem(BattleState state) : base(state)
    {
        _particles = new List<SourceParticle>();
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
        for (int i = 0; i < _particles.Count; i++)
        {
            if (_particles[i].Run()) continue;

            _particles.RemoveAt(i);
        }
    }

    public void Add(SourceParticle particle)
    {
        _particles.Add(particle);
    }

    public override void FixedRun()
    {
    }

    public override void Init()
    {

    }

}
