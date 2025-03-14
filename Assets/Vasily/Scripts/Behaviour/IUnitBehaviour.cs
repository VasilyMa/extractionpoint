public interface IUnitBehaviour
{
    IUnitBehaviour Init(PhotonEnemy owner);
    void Enter();
    void Exit();
    void Run();
}
