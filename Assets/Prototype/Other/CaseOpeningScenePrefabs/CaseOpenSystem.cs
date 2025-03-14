using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseOpenSystem : MonoBehaviour
{
    Animator _anim;

    public void Start()
    {
        _anim = GetComponent<Animator>();   
    }
    public void OpenCase()
    {
        _anim.CrossFade("CaseOpen", 0);
    }
}
