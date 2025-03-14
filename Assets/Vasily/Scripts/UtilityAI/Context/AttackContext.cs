using Client;
public struct AttackContext : IContext
{
    private Profile _profile;
    private float ContextScore;

    float IContext.ContextScore { get => ContextScore; set => ContextScore = value; }

    public void Init(Profile profile)
    {
        _profile = profile;
        _profile.UnitBrain.statesScore.Add(AIState.Attack, 0);
    }

    public void Run()
    {
        ContextScore = 0.3f;
        _profile.UnitBrain.statesScore[AIState.Attack] = ContextScore;  
    }
}
