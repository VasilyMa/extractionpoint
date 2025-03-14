using Cinemachine;
using UnityEngine;

public class CameraSwitchBlend : MonoBehaviour
{
    public CinemachineVirtualCamera VirtualCameraMain;
    public CinemachineVirtualCamera VirtualCameraLeft;
    public CinemachineVirtualCamera VirtualCameraRight;

    private void Start()
    {
        LobbyState.Instance.OnSwipeRight += () => {  };
    }

    void SwitchTo()
    {

    }

    public void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            VirtualCameraMain.Priority = 20;
            VirtualCameraLeft.Priority = 1;
            VirtualCameraRight.Priority = 1;
        }if (Input.GetButtonDown("Fire2"))
        {
            VirtualCameraMain.Priority = 1;
            VirtualCameraLeft.Priority = 20;
            VirtualCameraRight.Priority = 1;
        }if (Input.GetButtonDown("Fire3"))
        {
            VirtualCameraMain.Priority = 1;
            VirtualCameraLeft.Priority = 1;
            VirtualCameraRight.Priority = 20;
        }
    }
}
