using UnityEngine;

[System.Serializable]
public struct MinMaxValue 
{
    [Tooltip("Min resolve value")] public float Min;
    [Tooltip("Max resolve value")] public float Max;

    public MinMaxValue(float min, float max)
    {
        Min = min; 
        Max = max;
    }
}
