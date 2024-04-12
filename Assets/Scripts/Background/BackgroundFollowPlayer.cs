using UnityEngine;

public class BackgroundFollowPlayer : MonoBehaviour
{
    public Transform PlayerTransform; // 플레이어의 Transform을 참조할 변수

    void Start() {
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        PlayerTransform = Player.transform;
    }
    void Update()
    {
        if (PlayerTransform != null)
        {
            // 배경의 위치를 플레이어의 위치로 설정하여 플레이어를 따라다니도록 함
            transform.position = new Vector3(PlayerTransform.position.x, PlayerTransform.position.y, transform.position.z);
        }
    }
}
