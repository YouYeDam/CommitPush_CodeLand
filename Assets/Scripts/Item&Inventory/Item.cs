using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    public enum ItemType  // 아이템 유형
    {
        Equipment, // 장비
        Used, // 소비
        Quest, // 퀘스트
        ETC, //기타
    }

    public string ItemName; // 아이템의 이름
    public ItemType Type; // 아이템 유형
    public Sprite ItemImage; // 아이템의 이미지(인벤 토리 안에서 띄울)
    public GameObject ItemPrefab;  // 아이템의 프리팹 (아이템 생성시 프리팹으로 찍어냄)
    public string EquipmentType;  // 장비 유형
    public string UsedType;  // 소비 유형

    public bool IsAlreadyGet = false; // 아이템 중복 습득 방지

}