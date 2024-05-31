using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuffController : MonoBehaviour
{
    PlayerStatus PlayerStatus;
    PlayerMovement PlayerMovement;

    void Start() {
        PlayerStatus = GetComponent<PlayerStatus>();
        PlayerMovement = GetComponent<PlayerMovement>();
    }

    public void PlayerStatusUp(float BuffDuration, int HP, int MP, int ATK, int DEF, float AP, float Crit) { // 스탯업 버프
        PlayerStatus.PlayerMaxHP += HP;
        PlayerStatus.PlayerCurrentHP += HP;
        PlayerStatus.PlayerMaxMP += MP;
        PlayerStatus.PlayerCurrentMP += MP;
        PlayerStatus.PlayerATK += ATK;
        PlayerStatus.PlayerDEF += DEF;
        PlayerStatus.PlayerAP += AP;
        PlayerStatus.PlayerCrit += Crit;

        // PlayerStatusDown 메서드를 BuffDuration 후에 호출하고, 인수를 전달합니다.
        StartCoroutine(RemoveBuffAfterDuration(BuffDuration, HP, MP, ATK, DEF, AP, Crit));
    }

    public void PlayerStatusDown(int HP, int MP, int ATK, int DEF, float AP, float Crit) {
        PlayerStatus.PlayerMaxHP -= HP;
        PlayerStatus.PlayerMaxMP -= MP;
        PlayerStatus.PlayerATK -= ATK;
        PlayerStatus.PlayerDEF -= DEF;
        PlayerStatus.PlayerAP -= AP;
        PlayerStatus.PlayerCrit -= Crit;

        if (PlayerStatus.PlayerCurrentHP > PlayerStatus.PlayerMaxHP) {
            PlayerStatus.PlayerCurrentHP = PlayerStatus.PlayerMaxHP;
        }
        if (PlayerStatus.PlayerCurrentMP > PlayerStatus.PlayerMaxMP) {
            PlayerStatus.PlayerCurrentMP = PlayerStatus.PlayerMaxMP;
        }
    }

    private IEnumerator RemoveBuffAfterDuration(float Duration, int HP, int MP, int ATK, int DEF, float AP, float Crit) {
        yield return new WaitForSeconds(Duration);
        PlayerStatusDown(HP, MP, ATK, DEF, AP, Crit);
    }

    public void PlayerSpeedUp(float BuffDuration, float MoveSpeedBuff, float JumpSpeedBuff, float ClimbSpeedBuff) {
        PlayerMovement.SpeedChange(BuffDuration, MoveSpeedBuff, JumpSpeedBuff, ClimbSpeedBuff);
    }
}
