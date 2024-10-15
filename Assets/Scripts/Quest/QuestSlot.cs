using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class QuestSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text QuestNameText;
    public string QuestName;
    private Quest Quest;
    private QuestToolTip QuestToolTip;
    
    void Start() {
        GameObject ToolTipObject = GameObject.Find("QuestToolTip");

        if (ToolTipObject != null) {
            QuestToolTip = ToolTipObject.GetComponent<QuestToolTip>();
        }
    }

    public void AddQuest(Quest quest) { // 퀘스트 슬롯에 등록
        this.Quest = quest;
        QuestName = quest.Title;
        QuestNameText.text = QuestName;
        UpdateQuestStatus();
    }

    public void UpdateQuestStatus() { // 퀘스트 상태 갱신
        if (Quest.IsCompleted) {
            QuestNameText.color = Color.gray; // 완료된 퀘스트는 회색
        } 
        else if (Quest.IsReadyToComplete) {
            QuestNameText.color = Color.yellow; // 완료 가능한 퀘스트는 노란색
        } 
        else {
            QuestNameText.color = Color.white; // 기본 상태의 퀘스트는 흰색
        }
    }

    public void OnPointerEnter(PointerEventData eventData) { // 마우스가 퀘스트 슬롯 위에 있을 경우 툴팁 전시
        if (Quest != null) {
            QuestToolTip.ShowToolTip(Quest);
        }
    }

    public void OnPointerExit(PointerEventData eventData) { // 마우스가 퀘스트 슬롯을 벗어나면 툴팁 숨김
        QuestToolTip.HideToolTip();
    }
}
