using UnityEngine;

public class AnimObjectInstancer : MonoBehaviour
{
    public GameObject go;
    public Transform position;

    public void InstanceGO()
    {
        GameObject.Instantiate(go,position.position,Quaternion.identity);
    }
}
