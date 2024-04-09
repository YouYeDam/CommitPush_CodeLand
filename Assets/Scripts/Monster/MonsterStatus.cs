using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStatus : MonoBehaviour
{
    [SerializeField] public string MonsterName; // 플레이어 이름
    [SerializeField] public float MonsterMaxHealth;
    [SerializeField] public float MonsterCurrentHealth;
    [SerializeField] public float MonsterDamage = 10f;
    [SerializeField] public int MonsterEXP = 1;

    void Start()
    {
        MonsterCurrentHealth = MonsterMaxHealth;
    }

}
