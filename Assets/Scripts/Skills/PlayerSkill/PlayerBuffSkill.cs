using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuffSkill : MonoBehaviour
{
    [SerializeField] float DestroyDelay = 0.5f;
    GameObject Player;
    PlayerStatus PlayerStatus;
    PlayerBuffController PlayerBuffController;
    public float CoolDown = 60f;
    public int MPUse = 0;
    public float BuffDuration = 0f;
    float PlayerX;
    [SerializeField] float YSpeed;
    
    [SerializeField] bool IsStatusUpBuff = false; //스탯업 버프인지
    [SerializeField] int HPBuff = 0;
    [SerializeField] int MPBuff = 0;
    [SerializeField] int ATKBuff = 0;
    [SerializeField] int DEFBuff = 0;
    [SerializeField] int APBuff = 0;
    [SerializeField] int CritBuff = 0;

    void Start() {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
        PlayerBuffController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBuffController>();

        CheckBuffType(); // 버프 타입 체크

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

    void CheckBuffType() {
        if (IsStatusUpBuff) {
            PlayerBuffController.PlayerStatusUp(BuffDuration, HPBuff, MPBuff, ATKBuff, DEFBuff, APBuff, CritBuff);
        }
    }
    void DestroySelf() {
            Destroy(gameObject);
    }
}
