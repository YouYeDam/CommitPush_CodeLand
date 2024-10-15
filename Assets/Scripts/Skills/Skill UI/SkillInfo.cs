using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInfo : MonoBehaviour
{
    public string SkillName;
    public string SkillType; //공격형, 버프형 등
    public string SkillCoolDown;
    public string SkillMPUse;
    [TextArea(4, 15)] public string SkillExplain; // 스킬 설명 텍스트 박스
}
