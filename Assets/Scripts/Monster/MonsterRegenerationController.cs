using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRegenerationController : MonoBehaviour
{
    [SerializeField] float DelayTime = 10f;

    public void RegenerateMonster(GameObject MonsterObject, Vector3 MonsterPosition, Quaternion MonsterRotation) { // 몬스터 생성 함수
        GameObject MonsterInstance = Instantiate(MonsterObject, MonsterPosition, MonsterRotation);

        BasicMonsterMovement BasicMonsterMovement = MonsterInstance.GetComponent<BasicMonsterMovement>();
        BasicMonsterMovement.IsTakeDamge = false;
        BasicMonsterMovement.CanWalk = true;
        MonsterInstance.SetActive(false);
        StartCoroutine(RegenerateMonsterWithDelay(MonsterInstance));
    }

    IEnumerator RegenerateMonsterWithDelay(GameObject MonsterInstance) {
        yield return new WaitForSeconds(DelayTime); // DelayTime만큼 대기
        MonsterInstance.SetActive(true);
    }
}