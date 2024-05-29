using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInfo : MonoBehaviour
{
    public string SkillName;
    public string SkillType; //공격형, 버프형
    public string SkillMPUse;
    public string SkillCoolDown;
    [TextArea] public string SkillExplain;
}
