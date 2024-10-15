using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestToolTip : MonoBehaviour
{
    [SerializeField] GameObject QuestToolTipBase;
    [SerializeField] TMP_Text QuestNameText;
    [SerializeField] TMP_Text QuestNPCText;
    [SerializeField] TMP_Text QuestPlaceText;
    [SerializeField] TMP_Text QuestDescText;
    [SerializeField] TMP_Text QuestProgressText;

    public void ShowToolTip(Quest quest) { // 퀘스트 툴팁 전시
        QuestToolTipBase.SetActive(true);

        if (quest == null) {
            return;
        }

        QuestNameText.text = quest.Title;
        QuestDescText.text = quest.QuestDescription;
        QuestNPCText.text = "NPC 이름: " + quest.NPCName;
        QuestPlaceText.text = "위치: " + quest.Place;
        QuestProgressText.text = "";

        
        foreach (var objective in quest.Objectives) { // 각 목표의 진행 상황을 텍스트에 추가
            if (quest.IsCompleted) {
                objective.CurrentAmount = objective.RequiredAmount; // 퀘스트가 완료된 상태라면 현재 수량을 요구 수량과 동일하게 설정
            }

            if (quest.Objectives.TrueForAll(obj => obj.Type == QuestObjective.ObjectiveType.None)) { // 퀘스트 목표가 없는 경우 NPC와 대화하는 것이므로 해당 텍스트로 출력
                QuestProgressText.text = quest.NPCName + "과 대화하기";
            }
            else { // 그 외에는 "현재 수량 / 요구 수량" 형식으로 텍스트 출력
            QuestProgressText.text += $"{objective.TargetName}: {objective.CurrentAmount}/{objective.RequiredAmount}\n";
            }
        }
    }

    public void HideToolTip() { // 퀘스트 툴팁 숨김
        QuestToolTipBase.SetActive(false);
    }
}
