using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemDrop {
    public GameObject ItemPrefab; // 드롭될 아이템의 프리팹
    public float SpawnProbability;
}

public class MonsterDropItem : MonoBehaviour {
    public ItemDrop[] ItemDrops; // 아이템 드롭 데이터 배열
    private Vector3 MyPosition; // 몬스터의 위치
    LayerMask GroundLayer;

    DropMoney DropMoney;
    GameObject MoneyInstance;
    [SerializeField] public int MonsterBit = 0;
    [SerializeField] public int MonsterSnippet = 0;
    void Start() {
        GroundLayer = LayerMask.GetMask("Ground");
        MonsterBit = Mathf.FloorToInt(MonsterBit * Random.Range(1.0f, 1.51f));
    }
    void Update() {
        MyPosition = transform.position; // 매 프레임마다 몬스터의 현재 위치를 업데이트
    }

    public void DropItems() { // 몬스터의 아이템 드랍 기능
        float BaseSpacing = 0.3f; // 기본 아이템 간의 간격
        int Direction = 1; // 아이템 생성 방향 초기화 (1은 오른쪽, -1은 왼쪽)
        float CurrentSpacing = 0.0f; // 추가 간격

        for (int i = 0; i < ItemDrops.Length; i++) { // 아이템 여러 개 드랍 시 겹치지 않게 하기 위해
            Vector3 Offset;
            if (i == 0) {
                Offset = Vector3.zero; // 첫 번째 아이템은 정중앙에
            } 
            else {
                CurrentSpacing = (i + 1) / 2 * BaseSpacing; // 두 번째 아이템부터 간격 증가
                Offset = new Vector3(CurrentSpacing * Direction, 0, 0); // 위치 계산
                Direction *= -1; // 방향 전환(한 쪽으로 쏠림 방지)
            }
            Vector3 SpawnPosition = MyPosition + Offset;

            if (IsGroundNearby(MyPosition)) {
                SpawnPosition = MyPosition; // Ground 레이어가 감지되면 몬스터의 위치에 아이템을 생성 (아이템이 벽을 뚫고 드랍되는 것을 방지)
            }

            if (ItemDrops[i].ItemPrefab != null && Random.value <= ItemDrops[i].SpawnProbability) { // 아이템 드랍 확률을 만족하면 아이템 드랍

                if (ItemDrops[i].ItemPrefab.tag == "Money") { // 화폐 태그일 경우
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

    bool IsGroundNearby(Vector3 position) { // 레이캐스트를 이용해 Ground 레이어가 근처에 있는지 검사
        float CheckDistance = 0.6f; // 검사할 거리
        RaycastHit2D HitRight = Physics2D.Raycast(position, Vector2.right, CheckDistance, GroundLayer);
        RaycastHit2D HitLeft = Physics2D.Raycast(position, Vector2.left, CheckDistance, GroundLayer);

        if (HitRight.collider != null || HitLeft.collider != null) { // Ground 레이어 감지 시 true 반환
            return true;
        }
        return false;
    }
}

