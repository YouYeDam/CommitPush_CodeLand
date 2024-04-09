using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public string PlayerName; // 플레이어 이름
    [SerializeField] public int PlayerLevel = 1;
    [SerializeField] public float PlayerMaxHealth = 100f;
    [SerializeField] public float PlayerCurrentHealth;
    [SerializeField] public int PlayerNextEXP = 10;
    [SerializeField] public int PlayerCurrentEXP = 0;

    void Start() {
        PlayerCurrentHealth = PlayerMaxHealth;
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
