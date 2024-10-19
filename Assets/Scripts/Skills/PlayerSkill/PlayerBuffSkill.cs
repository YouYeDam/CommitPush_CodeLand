using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuffSkill : MonoBehaviour
{
    [SerializeField] float DestroyDelay = 0.5f;
    GameObject Player;
    PlayerBuffController PlayerBuffController;
    public float CoolDown = 60f;
    public int MPUse = 0;
    public float BuffDuration = 0f;
    [SerializeField] bool IsStatusUpBuff = false; //스탯업 버프인지
    [SerializeField] int HPBuff = 0;
    [SerializeField] int MPBuff = 0;
    [SerializeField] int ATKBuff = 0;
    [SerializeField] int DEFBuff = 0;
    [SerializeField] float APBuff = 0f;
    [SerializeField] float CritBuff = 0f;

    [SerializeField] bool IsSpeedUpBuff = false; // 플레이어 이동속도업 버프인지
    [SerializeField] float MoveSpeedBuff = 1f;
    [SerializeField] float JumpSpeedBuff = 1f;
    [SerializeField] float ClimbSpeedBuff = 1f;


    void Start() {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerBuffController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBuffController>();

        CheckBuffType(); // 버프 타입 체크

        Invoke("DestroySelf", DestroyDelay); 
    }

    void Update() {
        float PlayerX = Player.transform.position.x;
        float PlayerY = Player.transform.position.y + 1.5f;

        transform.position = new Vector3(PlayerX, PlayerY, transform.position.z);
    }

    void CheckBuffType() { // 버프 타입 체크
        if (IsStatusUpBuff) {
            PlayerBuffController.PlayerStatusUp(BuffDuration, HPBuff, MPBuff, ATKBuff, DEFBuff, APBuff, CritBuff); // 할당된 수치에 맞게 스탯 상승
        }
        if (IsSpeedUpBuff) {
            PlayerBuffController.PlayerSpeedUp(BuffDuration, MoveSpeedBuff, JumpSpeedBuff, ClimbSpeedBuff);
        }
    }
    
    void DestroySelf() { // 스킬 파괴
            Destroy(gameObject);
    }
}
