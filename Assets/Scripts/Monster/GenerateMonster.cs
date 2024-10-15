using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMonster : MonoBehaviour
{
    [SerializeField] private List<GameObject> GeneratedMonsters; // 여러 몬스터 프리팹을 저장할 리스트
    private Vector3 MyPosition; // 소환 스킬을 사용하는 몬스터의 위치
    [SerializeField] float DelayTime = 0.5f; // 소환된 몬스터 데미지 트리거 대기 시간
    BasicMonsterMovement BasicMonsterMovement;
    MonsterStatus MonsterStatus;

    void Update() {
        MyPosition = transform.position; // 매 프레임마다 몬스터의 현재 위치를 업데이트
    }

    public void GenerateMonsters(Vector3 ? Position = null) { // 몬스터 생성 (Position을 못받았다면 null로 사용)
        Vector3 SpawnPosition = Position ?? MyPosition; // Position이 null이면 MyPosition 사용

        foreach (GameObject MonsterPrefab in GeneratedMonsters) {
            if (MonsterPrefab != null) {
                GameObject MonsterInstance = Instantiate(MonsterPrefab, SpawnPosition, Quaternion.identity);
                BasicMonsterMovement = MonsterInstance.GetComponent<BasicMonsterMovement>();
                MonsterStatus = MonsterInstance.GetComponent<MonsterStatus>();
                MonsterStatus.IsSummonedMonster = true;
                StartCoroutine(RegenerateMonsterWithDelay(BasicMonsterMovement));
            }
        }
    }

    IEnumerator RegenerateMonsterWithDelay(BasicMonsterMovement GenMonsterBasicMonsterMovement) { // 소환된 몬스터의 데미지 트리거 대기루틴
        GenMonsterBasicMonsterMovement.CanTriggerDamage = false;
        yield return new WaitForSeconds(DelayTime);
        GenMonsterBasicMonsterMovement.CanTriggerDamage = true;
    }
}
