using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMonster : MonoBehaviour
{
    [SerializeField] private List<GameObject> GeneratedMonsters; // 여러 프리팹을 저장할 리스트
    private Vector3 MyPosition; // 몬스터의 위치
    [SerializeField] float DelayTime = 0.5f;
    BasicMonsterMovement BasicMonsterMovement;
    MonsterStatus MonsterStatus;
    void Update() {
        MyPosition = transform.position; // 매 프레임마다 몬스터의 현재 위치를 업데이트
    }

    public void GenerateMonsters() {
        foreach (GameObject MonsterPrefab in GeneratedMonsters) {
            if (MonsterPrefab != null) {
                GameObject MonsterInstance = Instantiate(MonsterPrefab, MyPosition, Quaternion.identity);
                MonsterStatus = MonsterInstance.GetComponent<MonsterStatus>();
                BasicMonsterMovement = MonsterInstance.GetComponent<BasicMonsterMovement>();
                MonsterStatus.IsBossMonster = true;
                StartCoroutine(RegenerateMonsterWithDelay(BasicMonsterMovement));
                
            }
        }
    }

    IEnumerator RegenerateMonsterWithDelay(BasicMonsterMovement GenMonsterBasicMonsterMovement) {
        GenMonsterBasicMonsterMovement.CanTriggerDamage = false;
        yield return new WaitForSeconds(DelayTime);
        GenMonsterBasicMonsterMovement.CanTriggerDamage = true;
    }
}
