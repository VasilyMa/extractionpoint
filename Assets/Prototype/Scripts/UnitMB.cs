using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
using Client;

[RequireComponent(typeof(Rigidbody),typeof(CapsuleCollider),typeof(Animator))]
public class UnitMB : MonoBehaviour
{
    [SerializeField] private Weapon _weapon;
    [SerializeField] private Transform _footL;
    [SerializeField] private Transform _footR;
    [SerializeField] private Animator _animator;


    public void Start()
    {
       
    }


    #region Weapon
    /*void InitWeapon()
    {
        _weaponController.Init();
    }
    public Weapon GetCurrentWeapon()
    {
        return _weapon;
    }*/
    #endregion
    public void FootL()
    {
        FootPrintManager.Instance.Print(_footL, transform.position + transform.forward);
    }
    public void FootR()
    {
        FootPrintManager.Instance.Print(_footR, transform.position + transform.forward);
    }
    /*public void CompleteReload()
    {
        _weaponController.CompleteReload();
        
    }*/
    private void OnDrawGizmos()
    {
       /* if (Application.isPlaying)
        {
            Gizmos.color = MoveToTargetColor;
            Gizmos.DrawSphere(_movementController.GetMoveInput() * 2f + transform.position, 0.2f);
            Gizmos.color = ViewColor;
            Gizmos.DrawSphere(transform.forward * 2f + transform.position, 0.2f);
        }*/
    }
}
