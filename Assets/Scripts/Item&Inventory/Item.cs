using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Item")]
public class Item : ScriptableObject
{
    public enum ItemType  // 아이템 유형
    {
        Equipment, // 장비
        Used, // 소비
        SourceCode, // 소스코드
        ETC, // 기타
        Quest, // 퀘스트
    }

    public string ItemName; // 아이템의 이름
    public string ItemDetailType; // 아이템의 세부타입
    public int ItemCount = 1; // 아이템 개수
    public int ItemCost = 0; // 아이템 가격
    public ItemType Type; // 아이템 유형
    public Sprite ItemImage; // 아이템의 이미지
    public GameObject ItemPrefab;  // 아이템의 프리팹
    public bool IsAlreadyGet = false; // 아이템 중복 습득 방지
    [TextArea(4, 15)] public string ItemInfo;
}