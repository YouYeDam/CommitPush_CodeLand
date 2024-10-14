using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevivePosition : MonoBehaviour
{
    GameObject Player;
    PlayerMovement PlayerMovement;

    void Start() {
    PlayerMovement = FindObjectOfType<PlayerMovement>();
    Player = GameObject.FindGameObjectWithTag("Player");
        if (!PlayerMovement.IsAlive) {
            SetPlayerPosition();
        }
    }

    void SetPlayerPosition() { // 부활 장소의 특정 리스폰 스폿에서 부활
        PlayerMovement.IsAlive = true;
        PlayerMovement.MyAnimator.SetBool("IsDying", false);
        Player.transform.position = transform.position;
    }
}
