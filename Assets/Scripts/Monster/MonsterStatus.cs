using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStatus : MonoBehaviour
{
    [SerializeField] public string MonsterName; // 플레이어 이름
    [SerializeField] public float MonsterMaxHealth;
    [SerializeField] public float MonsterCurrentHealth;

    void Start()
    {
        MonsterCurrentHealth = MonsterMaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        DeathCheck();
    }

    void DeathCheck() {
        if (MonsterCurrentHealth > 0) {
            return;
        }
        Destroy(gameObject);
    }
}
