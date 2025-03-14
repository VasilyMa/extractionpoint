using Sirenix.OdinInspector;
using Client;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AIProfile", menuName = "Configs/AIProfile")]
public class AIProfile : ScriptableObject
{ 
    public Profile Profile;

    public Profile GetProfile()
    {
        return Profile;
    }
}

[System.Serializable]
public class Profile
{
    public AnimationCurve AggressionScoreByHealth;
    public AnimationCurve AggressionScoreByDistance;
    public AnimationCurve AggressionScoreByAlliesCount;

    public AnimationCurve CowardiceScoreByHealth;
    public AnimationCurve CowardiceScoreByDistance;

    public AnimationCurve DefenceScoreByHealth;
    public AnimationCurve DefenceScoreByDistance;

    public AnimationCurve SupportScoreByAlliesAvarageHealth;

    public AnimationCurve SmartScoreByHealth;

    public UnitBrain UnitBrain;
    [SerializeReference]
    public IContext[] _contexts;
    public void Init(UnitMB unitMB)
    {
        UnitBrain = new UnitBrain();
        UnitBrain.statesScore = new Dictionary<AIState, float>();
        UnitBrain.DetectedUnits = new List<UnitMB>();
        UnitBrain.Transform = unitMB.transform;
        for (int i = 0; i < _contexts.Length; i++)
        {
            _contexts[i].Init(this);
        }
    }
    public void Run()
    {
        for (int i = 0; i < _contexts.Length; i++)
        {
            _contexts[i].Run();
        }
    }
}