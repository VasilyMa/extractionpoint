using UnityEngine;
using UnityEngine.AI;

public class DeadBehaviour : IUnitBehaviour
{
    PhotonEnemy _owner;
    Animator _animator;
    Collider _collider;
    NavMeshAgent _agent;

    public DeadBehaviour(PhotonEnemy owner)
    {
        _owner = owner;
        _animator = owner.GetComponentInChildren<Animator>();
        _collider = owner.GetComponent<Collider>();
        _agent = owner .GetComponent<NavMeshAgent>();
    }

    public IUnitBehaviour Init(PhotonEnemy owner)
    {
        return new DeadBehaviour(owner);
    }

    public void Enter()
    {
        _agent.ResetPath();
        _agent.enabled = false;
        _collider.enabled = false;
        _animator.enabled = false;
        _owner.SetRagdoll(true);
    }

    public void Exit()
    {
        _owner.SetRagdoll(false);
        _animator.enabled = true;
        _collider.enabled = true;
        _agent.enabled = true;
    }

    public void Run()
    {

    }
}
