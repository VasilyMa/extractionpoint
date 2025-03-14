using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ratator : MonoBehaviour
{
    [SerializeField] 
    public float RotationSpeed = 1.0f;  //
    public AxisType axisType;
    void Update()
    {
        if (axisType == AxisType.x)
        {
            float rotationAmount = RotationSpeed * Time.deltaTime;
            transform.Rotate(rotationAmount,0, 0);
        }
        if (axisType == AxisType.y)
        {
            float rotationAmount = RotationSpeed * Time.deltaTime;
            transform.Rotate(0, rotationAmount, 0);
        }
        if (axisType == AxisType.z)
        {
            float rotationAmount = RotationSpeed * Time.deltaTime;
            transform.Rotate(0, 0, rotationAmount);
        }
        
    }
    public enum AxisType {x,y,z }
}
