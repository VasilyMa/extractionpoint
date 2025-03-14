public interface IContext
{
    public float ContextScore { get; set; }
    public void Init(Profile profile);
    public void Run();
}
