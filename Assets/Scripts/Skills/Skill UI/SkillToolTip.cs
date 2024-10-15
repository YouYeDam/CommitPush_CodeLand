using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillToolTip : MonoBehaviour
{
   PlayerStatus PlayerStatus;
    [SerializeField] GameObject SkillToolTipBase;
    [SerializeField] TMP_Text SkillNameText;
    [SerializeField] TMP_Text SkillInfoText;
    [SerializeField] TMP_Text SkillMPUseText;
    [SerializeField] TMP_Text SkillTypeText;
    [SerializeField] TMP_Text SkillCoolDownText;
    [SerializeField] Image SkillImage;

    void Start() {
        PlayerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
    }

    public void ShowToolTip(GameObject SkillPrefab) { // 아이템 툴팁 전시 기능
        SkillToolTipBase.SetActive(true);

        if (SkillPrefab == null) {
            return;
        }

        SkillNameText.text = SkillPrefab.GetComponent<SkillInfo>().SkillName;
        SkillInfoText.text = SkillPrefab.GetComponent<SkillInfo>().SkillExplain;
        SkillMPUseText.text = "소모 정신력: " + SkillPrefab.GetComponent<SkillInfo>().SkillMPUse + " MP";
        SkillTypeText.text = "스킬 유형: " + SkillPrefab.GetComponent<SkillInfo>().SkillType;
        SkillCoolDownText.text = "쿨타임: " + SkillPrefab.GetComponent<SkillInfo>().SkillCoolDown + "초";
        SpriteRenderer SkillSpriteRenderer = SkillPrefab.GetComponent<SpriteRenderer>();
        SkillImage.sprite = SkillSpriteRenderer.sprite;
    }

    public void HideToolTip() { // 스킬 툴팁 숨김 기능
        SkillToolTipBase.SetActive(false);
    }
}
