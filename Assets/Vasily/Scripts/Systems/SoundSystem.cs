using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : SourceSystem
{
    public static SoundSystem Instance { get; private set; }
    protected List<SourceSound> _sounds;

    public SoundSystem(BattleState state) : base(state)
    {
        _sounds = new List<SourceSound>();
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
        for (int i = 0; i < _sounds.Count; i++)
        {
            if (_sounds[i].Run()) continue;

            _sounds.RemoveAt(i);
        }
    }

    public void Add(SourceSound sound)
    {
        _sounds.Add(sound);
    }

    public override void FixedRun()
    {
    }

    public override void Init()
    {

    }

}
