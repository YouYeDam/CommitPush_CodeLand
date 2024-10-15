using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Item")] // 아이템 메뉴 생성
public class Item : ScriptableObject
{
    public enum ItemType  // 아이템 유형
    {
        Equipment, // 장비
        Used, // 소비
        SourceCode, // 소스코드 (스킬북)
        ETC, // 기타
        Quest, // 퀘스트
    }

    public string ItemName;
    public string ItemDetailType;
    public int ItemCount = 1;
    public int ItemCost = 0;
    public ItemType Type;
    public Sprite ItemImage;
    public GameObject ItemPrefab;
    public bool IsAlreadyGet = false; // 아이템 중복 습득 방지 검사
    [TextArea(4, 15)] public string ItemInfo;
}