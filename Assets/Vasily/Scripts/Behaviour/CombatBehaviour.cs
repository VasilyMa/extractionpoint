using System.Collections.Generic;

using UnityEngine;

public class CombatBehaviour : IUnitBehaviour
{
    private PhotonEnemy _owner;
    private Animator _animator;
    private float _timeToAttack;

    public CombatBehaviour(PhotonEnemy owner)
    {
        _owner = owner;
        _animator = owner.GetComponentInChildren<Animator>();
    }

    public IUnitBehaviour Init(PhotonEnemy owner)
    {
        return new CombatBehaviour(owner);
    }
    public void Enter()
    {

    }

    public void Exit()
    {

    }

    public void Run()
    {
        if (_owner.Target.IsDie)
        {
            _animator.ResetTrigger("Attack");

            if (_owner.GetNearestTarget() == null) 
                _owner.SetBehaviour(_owner.GetBehaviour<IdleBehaviour>()); 
            else 
                _owner.SetBehaviour(_owner.GetBehaviour<MoveBehaviour>());

            return;
        }

        _timeToAttack -= Time.deltaTime;

        if (_timeToAttack <= 0)
        {
            _animator.SetTrigger("Attack");
            _timeToAttack = 1f;
        }

        float distanceToAttack = _owner.GetDistanceToTarget();

        if (distanceToAttack > 2f)
        {
            _animator.ResetTrigger("Attack");
            _owner.SetBehaviour(_owner.GetBehaviour<MoveBehaviour>());
        }

    }
}