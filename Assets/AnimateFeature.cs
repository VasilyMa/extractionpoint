using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateFeature : MonoBehaviour
{
    protected PhotonEnemy _owner;

    public AnimateFeature Init(PhotonEnemy owner)
    {
        _owner = owner; 

        return this;
    }

    public void ActionAttack() => _owner.ActionAttack();
}
