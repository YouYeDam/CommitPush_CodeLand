using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public string PlayerName; // 플레이어 이름
    public float PlayerMaxHealth = 100;
    public float PlayerCurrentHealth;
    
    public void SetPlayerName(string NewName)
    {
        PlayerName = NewName;
    }
}
