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

    void FindAndFollowPlayer() { // 카메라가 플레이어를 따라다닐 수 있도록
        GameObject PlayerObject = GameObject.FindGameObjectWithTag("Player");
        if (PlayerObject != null && VirtualCamera != null) {
            VirtualCamera.Follow = PlayerObject.transform;
        }
    }
}
