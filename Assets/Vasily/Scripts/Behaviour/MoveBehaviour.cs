using UnityEngine;
using UnityEngine.AI;

public class MoveBehaviour : IUnitBehaviour
{
    [SerializeField] protected float _distanceToAttack = 2f;
    PhotonEnemy _owner;
    Animator _animator;
    NavMeshAgent _agent;

    public MoveBehaviour(PhotonEnemy owner)
    {
        _owner = owner;
        _animator = owner.GetComponentInChildren<Animator>();
        _agent = owner.GetComponent<NavMeshAgent>();
    }

    public IUnitBehaviour Init(PhotonEnemy owner)
    {
        return new MoveBehaviour(owner);
    }

    public void Enter()
    {
        _animator.SetBool("IsMove", true);
    }

    public void Exit()
    {
        _animator.SetBool("IsMove", false);
    }

    public void Run()
    {
        if (_owner.MoveDestination())
        {
            if (_owner.GetDistanceToTarget() < _distanceToAttack)
            {
                _owner.SetBehaviour(_owner.GetBehaviour<CombatBehaviour>());
            }
        }
        else
        {
            _owner.SetBehaviour(_owner.GetBehaviour<IdleBehaviour>());
        }
    }
}
