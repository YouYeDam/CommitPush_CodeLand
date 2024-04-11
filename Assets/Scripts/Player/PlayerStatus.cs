using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public string PlayerName; // 플레이어 이름
    public string PlayerClass = "개발자";
    [SerializeField] public int PlayerLevel = 1;
    [SerializeField] public int PlayerMaxHP = 100;
    [SerializeField] public int PlayerCurrentHP;
    [SerializeField] public int PlayerMaxMP = 100;
    [SerializeField] public int PlayerCurrentMP = 100;
    [SerializeField] public int PlayerMaxStamina = 100;
    [SerializeField] public int PlayerCurrentStamina = 100;
    [SerializeField] public int PlayerNextEXP = 10;
    [SerializeField] public int PlayerCurrentEXP = 0;

    [SerializeField] public int PlayerATK = 1;
    [SerializeField] public int PlayerDEF = 0;
    [SerializeField] public float PlayerCrit = 0f;

    void Start() {
        PlayerCurrentHP = PlayerMaxHP;
    }
    public void SetPlayerName(string NewName){
            PlayerName = NewName;
    }

    public void GainEXP(int EXP) {
        PlayerCurrentEXP += EXP;
        if (PlayerCurrentEXP >= PlayerNextEXP) {
            LevelUp();
            PlayerNextEXP = PlayerNextEXP + (int)Mathf.Floor(PlayerNextEXP * 1.5f);
        }
    }

    void LevelUp() {
        PlayerLevel += 1;
    }
}
