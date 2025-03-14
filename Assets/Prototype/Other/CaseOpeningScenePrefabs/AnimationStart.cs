using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStart : MonoBehaviour
{
    Animator _anim;
    float timer;
    float remainingTime;

    public string AnimationStateName;
    public float minTime;
    public float maxTime;
    // Start is called before the first frame update
    void Start()
    {
        _anim =  GetComponent<Animator>();
        timer = GetRemainingTime();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            _anim.CrossFade(AnimationStateName, 0);
            timer = GetRemainingTime();
        }
    }
    float GetRemainingTime()
    {
        return Random.Range(minTime, maxTime);
    }
}
