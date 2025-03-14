using Client;
using UnityEngine;
public struct MoveToContext : IContext
{
    private float ContextScore;
    private Profile _profile;
    float IContext.ContextScore { get => ContextScore; set => ContextScore = value; }

    public void Init(Profile profile)
    {
        _profile = profile;
        _profile.UnitBrain.statesScore.Add(AIState.MoveTo, 0);
    }

    public void Run()
    {
        var distance = float.MaxValue;

        for (int i = 0; i < _profile.UnitBrain.DetectedUnits.Count; i++)
        {
            var distanceToEnemy = Vector3.Distance(_profile.UnitBrain.Transform.position, _profile.UnitBrain.DetectedUnits[i].transform.position);
            if (distanceToEnemy < distance)
            {
                distance = distanceToEnemy;
                _profile.UnitBrain.priorityPointToMove = _profile.UnitBrain.DetectedUnits[i].transform.position;
                _profile.UnitBrain.priorityPointToLook = _profile.UnitBrain.DetectedUnits[i].transform.position;
                ContextScore = 0.4f;
            }
        }
        _profile.UnitBrain.statesScore[AIState.MoveTo] = ContextScore;
    }
}
