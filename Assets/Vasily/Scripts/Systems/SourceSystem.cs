public abstract class SourceSystem
{
    protected BattleState _state;

    public SourceSystem(BattleState state)
    {
        _state = state;
    }
    /// <summary>
    /// Invoke before game ready on client
    /// </summary>
    public abstract void Init();
    /// <summary>
    /// Invoke when all players ready and game start
    /// </summary>
    public abstract void AfterInit();
    public abstract void Run();
    public abstract void FixedRun();
    public abstract void Dispose();
}
