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

    void SetPlayerPosition() {
        PlayerMovement.IsAlive = true;
        PlayerMovement.MyAnimator.SetBool("IsDying", false);
        //PlayerMovement.MyAnimator.SetBool();
        Player.transform.position = transform.position;
    }
}
