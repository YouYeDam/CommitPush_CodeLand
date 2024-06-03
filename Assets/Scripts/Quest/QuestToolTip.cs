using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class QuestToolTip : MonoBehaviour
{
    [SerializeField] GameObject QuestToolTipBase;
    [SerializeField] TMP_Text QuestNameText;
    [SerializeField] TMP_Text QuestDescText;
    [SerializeField] TMP_Text QuestProgressText;

    public void ShowToolTip(Quest Quest) {
        QuestToolTipBase.SetActive(true);
        if (Quest == null) {
            return;
        }
        QuestNameText.text = Quest.Title;
        QuestDescText.text = Quest.Description;
    }

    public void HideToolTip() {
        QuestToolTipBase.SetActive(false);
    }
}
