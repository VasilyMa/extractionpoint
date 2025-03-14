using Client;

public struct IdleContext : IContext
{
    private float ContextScore;
    private Profile _profile;
    float IContext.ContextScore { get => ContextScore; set => ContextScore = value ; }

    public void Init(Profile profile)
    {
        _profile = profile;
        _profile.UnitBrain.statesScore.Add(AIState.Idle,0);
    }

    public void Run()
    {
        ContextScore = 0.2f;
        _profile.UnitBrain.statesScore[AIState.Idle] = ContextScore;
    }
}
