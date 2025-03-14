using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Weapon Data")]
public class WeaponHandsData : ScriptableObject
{
    public Transform leftHandTransform;
    public Transform rightHandTransform;

    public Vector3 leftHandPosition;
    public Quaternion leftHandRotation;
    public Vector3 rightHandPosition;
    public Quaternion rightHandRotation;

    public void SaveTransforms()
    {
        if (leftHandTransform != null)
        {
            leftHandPosition = leftHandTransform.localPosition;
            leftHandRotation = leftHandTransform.localRotation;
        }

        if (rightHandTransform != null)
        {
            rightHandPosition = rightHandTransform.localPosition;
            rightHandRotation = rightHandTransform.localRotation;
        }
    }
}
