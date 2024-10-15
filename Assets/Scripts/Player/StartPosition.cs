using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPosition : MonoBehaviour
{
    void Start()
    {
        GameObject Player = GameObject.Find("Player");
        
        if (Player != null)
        {
            // 플레이어 위치를 현재 스크립트가 할당된 오브젝트 위치로 설정
            Player.transform.position = transform.position;
        }
    }
}
