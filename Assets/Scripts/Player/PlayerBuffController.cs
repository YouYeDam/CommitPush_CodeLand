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

    public void PlayerStatusUp(float BuffDuration, int HP, int MP, int ATK, int DEF, int AP, int Crit) {
        PlayerStatus.PlayerMaxHP += HP;
        PlayerStatus.PlayerMaxMP += MP;
        PlayerStatus.PlayerATK += ATK;
        PlayerStatus.PlayerDEF += DEF;
        PlayerStatus.PlayerAP += AP;
        PlayerStatus.PlayerCrit += Crit;

        // PlayerStatusDown 메서드를 BuffDuration 후에 호출하고, 인수를 전달합니다.
        StartCoroutine(RemoveBuffAfterDuration(BuffDuration, HP, MP, ATK, DEF, AP, Crit));
    }

    public void PlayerStatusDown(int HP, int MP, int ATK, int DEF, int AP, int Crit) {
        PlayerStatus.PlayerMaxHP -= HP;
        PlayerStatus.PlayerMaxMP -= MP;
        PlayerStatus.PlayerATK -= ATK;
        PlayerStatus.PlayerDEF -= DEF;
        PlayerStatus.PlayerAP -= AP;
        PlayerStatus.PlayerCrit -= Crit;
    }

    private IEnumerator RemoveBuffAfterDuration(float duration, int HP, int MP, int ATK, int DEF, int AP, int Crit) {
        yield return new WaitForSeconds(duration);
        PlayerStatusDown(HP, MP, ATK, DEF, AP, Crit);
    }
}
