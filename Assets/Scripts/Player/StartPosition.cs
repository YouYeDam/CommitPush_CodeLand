using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPosition : MonoBehaviour
{
    void Start()
    {
        // "Player"라는 이름의 오브젝트를 찾음
        GameObject Player = GameObject.Find("Player");
        
        if (Player != null)
        {
            // player 오브젝트의 위치를 현재 스크립트가 할당된 gameObject의 위치로 설정
            Player.transform.position = transform.position;
        }
    }
}
