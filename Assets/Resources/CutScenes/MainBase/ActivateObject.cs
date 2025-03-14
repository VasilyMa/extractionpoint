using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateObject : MonoBehaviour
{
    public float Timer;
    public float Min;
    public float Max;
    public GameObject GO;
    private void Start()
    {
        Timer = Random.Range(Min, Max);
    }
    private void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0)
        {
            GO.SetActive(true);
        }
    }

}
