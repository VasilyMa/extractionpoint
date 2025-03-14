using UnityEngine;

public class IdleBehaviour : IUnitBehaviour
{
    private PhotonEnemy _owner;
    private Animator _animator;

    public IdleBehaviour(PhotonEnemy owner)
    {
        _owner = owner;
        _animator = owner.GetComponentInChildren<Animator>();
    }

    public IUnitBehaviour Init(PhotonEnemy owner)
    {
        return new IdleBehaviour(owner);
    }

    public void Enter()
    {
        _animator.SetBool("IsIdle", true);
    }

    public void Exit()
    {
        _animator.SetBool("IsIdle", false);
    }

    public void Run()
    {
        var target = _owner.GetNearestTarget();

        if (target != null)
        {
            _owner.SetBehaviour(_owner.GetBehaviour<MoveBehaviour>());
        }
    }
}
