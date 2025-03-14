using Client;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DecisionMakingSystem 
{
   /* List<UnitMB> _bots = new List<UnitMB>();
    List<Profile> _profiles = new List<Profile>();  
   

    public void InitBotsList(List<UnitMB> unitMBs)
    {
        _bots = unitMBs;    
    }
    public override void Init()
    {
    }

    public override void Run()
    {
        for (int i = 0; i < _profiles.Count; i++)
        {
            _profiles[i].Run();
        }
    }

    public override void Dispose()
    {
    }

    public override void FixedRun()
    {
        for (int i = 0; i < _bots.Count; i++)
        {
            if (!_bots[i].IsActiveBot) { continue; }

            AIState keyOfMaxValue = _profiles[i].UnitBrain.statesScore.OrderByDescending(entry => entry.Value).FirstOrDefault().Key;

            // for debug purposes only -->
            _profiles[i].UnitBrain.PriorityStateScore = _profiles[i].UnitBrain.statesScore[keyOfMaxValue];
            Debug.Log(keyOfMaxValue);
            // <--

            *//*switch (keyOfMaxValue)
            {
                case AIState.Idle:
                    TryAddRequestToEntity(_idleRequestPool.Value, entity);
                    break;
                case AIState.MoveTo:
                    TryAddRequestToEntity(_moveToTargetRequestPool.Value, entity);
                    break;
                case AIState.Attack:
                    TryAddRequestToEntity(_attackRequestPool.Value, entity);
                    break;
                case AIState.Defend:
                    TryAddRequestToEntity(_defendRequestPool.Value, entity);
                    break;
                case AIState.Support:
                    TryAddRequestToEntity(_supportRequestPool.Value, entity);
                    break;
                case AIState.KeepAtRange:
                    TryAddRequestToEntity(_keepAtRangeRequestPool.Value, entity);
                    break;
                case AIState.Terrorize:
                    TryAddRequestToEntity(_terrorizeRequestPool.Value, entity);
                    break;
            }*//*
            _profiles[i].UnitBrain.CurrentState = keyOfMaxValue;
        }
    }
    public void AddLogic(Profile profile, UnitMB unitMB)
    {
        _profiles.Add(profile);
        _bots.Add(unitMB);
        OnlineState.Instance.GetSystems<UnitBrainSystem>().AddUnitBrain(profile.UnitBrain);
    }*/
}
