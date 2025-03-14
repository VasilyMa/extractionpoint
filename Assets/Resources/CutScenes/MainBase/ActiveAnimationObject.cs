using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ActiveAnimationObject : MonoBehaviour
{
    float timer;
    float remainingTime;

    public float minTime;
    public float maxTime;
    public PlayableDirector playableDirector;
    // Start is called before the first frame update
    void Start()
    {
        timer = GetRemainingTime();
        playableDirector = GetComponent<PlayableDirector>();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            playableDirector.Play();
            timer = GetRemainingTime();
        }
    }
    float GetRemainingTime()
    {
        return Random.Range(minTime, maxTime);
    }
}

