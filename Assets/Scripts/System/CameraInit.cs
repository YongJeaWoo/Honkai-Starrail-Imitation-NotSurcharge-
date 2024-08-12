using Cinemachine;
using UnityEngine;

public class CameraInit : MonoBehaviour
{
    public void InitCameraSet(CinemachineFreeLook freeCam, GameObject player)
    {
        freeCam.Follow = player.transform;
        freeCam.LookAt = player.transform.GetChild(0);
    }
}
