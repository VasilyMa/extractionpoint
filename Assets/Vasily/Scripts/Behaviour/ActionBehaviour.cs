public class ActionBehaviour : IPhotonUnitBehaviour
{
    PhotonPlayer _player;

    public ActionBehaviour(PhotonPlayer player)
    {
        _player = player;
    }

    public void OnEnter()
    {

    }

    public void OnExit()
    {

    }

    public void OnRun()
    {
        _player.ActionAttack();
    }
}
