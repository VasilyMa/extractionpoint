using System.Collections.Generic;
using UnityEngine;
namespace Client
{
    public struct UnitBrain
    {
        public float PriorityStateScore;
        public Dictionary<AIState, float> statesScore;
        public AIState CurrentState;
        public Vector3 priorityPointToMove;
        public Vector3 priorityPointToLook;
        public List<UnitMB> DetectedUnits;
        public Transform Transform;
        /*public EcsPackedEntity bestAttackAvailable;
        public EcsPackedEntity bestSupportActionAvailable;
        public EcsPackedEntity bestDefensiveActionAvailable;
        public EcsPackedEntity bestTerrorizeActionAvailable;*/
    }
    public enum AIState
    {
        Idle,
        MoveTo,
        Attack,
        Defend,
        Support,
        Terrorize,
        KeepAtRange,
        None
    }
}