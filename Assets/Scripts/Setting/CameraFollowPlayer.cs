using UnityEngine;
using Cinemachine;

public class CameraFollowPlayer : MonoBehaviour
{
    private CinemachineVirtualCamera VirtualCamera;

    void Start()
    {
        VirtualCamera = GetComponent<CinemachineVirtualCamera>();
        FindAndFollowPlayer();
    }

    void FindAndFollowPlayer()
    {
        GameObject PlayerObject = GameObject.FindGameObjectWithTag("Player");
        if (PlayerObject != null && VirtualCamera != null)
        {
            VirtualCamera.Follow = PlayerObject.transform;
        }
    }
}
