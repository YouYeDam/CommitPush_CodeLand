using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemDrop {
    public GameObject ItemPrefab; // 드롭될 아이템의 프리팹
    public float SpawnProbability; // 아이템 드롭 확률
}

public class MonsterDropItem : MonoBehaviour {
    public ItemDrop[] ItemDrops; // 아이템 드롭 데이터 배열
    private Vector3 MyPosition; // 몬스터의 위치
    LayerMask GroundLayer; // Ground 레이어 마스크

    DropMoney DropMoney;
    GameObject MoneyInstance;
    [SerializeField] public int MonsterBit = 0;
    [SerializeField] public int MonsterSnippet = 0;
    void Start() {
        MonsterBit = Mathf.FloorToInt(MonsterBit * Random.Range(1.0f, 1.51f));
    }
    void Update() {
        MyPosition = transform.position; // 매 프레임마다 몬스터의 현재 위치를 업데이트
    }

    // 아이템 드롭 메서드
    public void DropItems() {
        float BaseSpacing = 0.3f; // 기본 아이템 간의 간격
        int Direction = 1; // 아이템 생성 방향 초기화 (1은 오른쪽, -1은 왼쪽)
        float CurrentSpacing = 0.0f; // 추가 간격

        for (int i = 0; i < ItemDrops.Length; i++) {
            Vector3 Offset;
            if (i == 0) {
                Offset = Vector3.zero; // 첫 번째 아이템은 정중앙에
            } else {
                CurrentSpacing = (i + 1) / 2 * BaseSpacing; // 두 번째 아이템부터 간격 증가
                Offset = new Vector3(CurrentSpacing * Direction, 0, 0); // 위치 계산
                Direction *= -1; // 방향 전환
            }
            Vector3 SpawnPosition = MyPosition + Offset;

            if (IsGroundNearby(MyPosition)) {
                SpawnPosition = MyPosition; // Ground가 감지되면 몬스터의 위치에 아이템을 생성
            }

            if (ItemDrops[i].ItemPrefab != null && Random.value <= ItemDrops[i].SpawnProbability) {

                if (ItemDrops[i].ItemPrefab.tag == "Money") {
                    MoneyInstance = Instantiate(ItemDrops[i].ItemPrefab, SpawnPosition, Quaternion.identity);
                    DropMoney = MoneyInstance.GetComponent<DropMoney>();
                    if (DropMoney.IsBit) {
                        DropMoney.Bit = MonsterBit;
                    }
                    else if (DropMoney.IsSnippet) {
                        DropMoney.Snippet = MonsterSnippet;
                    }
                }
                else {
                    Instantiate(ItemDrops[i].ItemPrefab, SpawnPosition, Quaternion.identity);
                }
            }
        }
    }

    // 레이캐스트를 이용해 Ground 레이어가 근처에 있는지 검사하는 메서드
    private bool IsGroundNearby(Vector3 position) {
        float CheckDistance = 3.0f; // 검사할 거리
        RaycastHit Hit;
        // 좌우로 레이캐스트
        if (Physics.Raycast(position, Vector3.right, out Hit, CheckDistance, GroundLayer) ||
            Physics.Raycast(position, Vector3.left, out Hit, CheckDistance, GroundLayer)) {
            return true;
        }
        return false;
    }
}

