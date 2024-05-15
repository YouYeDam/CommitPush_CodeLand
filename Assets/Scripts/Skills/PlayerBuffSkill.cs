using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuffSkill : MonoBehaviour
{
    [SerializeField] float DestroyDelay = 0.5f;
    [SerializeField] int EffectValue = 1;
    Rigidbody2D MyRigidbody;
    PlayerStatus PlayerStatus;
    GameObject Player;
    public float CoolDown = 30f;
    public int MPUse = 0;
    float PlayerX;
    [SerializeField] float YSpeed;

    void Start() {
        MyRigidbody = GetComponent<Rigidbody2D>();
        PlayerStatus = FindObjectOfType<PlayerStatus>();
        Player = GameObject.FindGameObjectWithTag("Player");
        Invoke("DestroySelf", DestroyDelay); 
    }

    void Update() {
        // 플레이어의 x 좌표를 가져오기
        float playerX = Player.transform.position.x;

        // 현재 오브젝트의 y 좌표를 YSpeed 만큼 업데이트하기
        float NewY = transform.position.y + (YSpeed * Time.deltaTime);

        // 현재 오브젝트의 x 좌표를 플레이어의 x 좌표로, y 좌표를 업데이트된 NewY로 설정
        transform.position = new Vector3(playerX, NewY, transform.position.z);
    }

    void SkillEffect() {
        PlayerStatus.PlayerATK *= EffectValue;
    }
    void DestroySelf() {
            Destroy(gameObject);
    }
}
