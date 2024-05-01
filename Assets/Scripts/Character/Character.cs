using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Character : MonoBehaviour
{
    [SerializeField] Sprite BluePlayerPortrait;
    [SerializeField] Sprite PinkPlayerPortrait;
    [SerializeField] Sprite WhitePlayerPortrait;

    public TMP_Text NameInfo;
    public TMP_Text ClassInfo;
    public TMP_Text LevelInfo;
    public TMP_Text HPInfo;
    public TMP_Text MPInfo;
    public TMP_Text ATKInfo;
    public TMP_Text DEFInfo;
    public TMP_Text APInfo;
    public TMP_Text CritInfo;
    public TMP_Text LevelUpPointInfo;
    public Image CharacterImage;

    PlayerStatus PlayerStatus;
    Animator PlayerAnimator;

    void Start() {
        PlayerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
        PlayerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }
    
    void Update() {
        NameInfo.text = "이름:" + PlayerStatus.PlayerName;
        ClassInfo.text = "클래스:" + PlayerStatus.PlayerClass;
        LevelInfo.text = "코딩력:" + PlayerStatus.PlayerLevel + "LV";
        HPInfo.text = "체력:" + PlayerStatus.PlayerMaxHP + "HP";
        MPInfo.text = "정신력:" + PlayerStatus.PlayerMaxMP + "MP";
        ATKInfo.text = "공격력:" + PlayerStatus.PlayerATK + "ATK";
        DEFInfo.text = "방어력:" + PlayerStatus.PlayerDEF + "DEF";
        APInfo.text = "가속력:" + PlayerStatus.PlayerAP * 100 + "%";
        CritInfo.text = "치명타:" + PlayerStatus.PlayerCrit * 100+ "%";
        LevelUpPointInfo.text = "스탯포인트: " + PlayerStatus.LevelUpPoint;
    }

    public void SetPortrait() {
        if (PlayerAnimator != null && PlayerAnimator.runtimeAnimatorController != null) {
            if (PlayerAnimator.runtimeAnimatorController.name == "Blue Player") {
                CharacterImage.sprite = BluePlayerPortrait;
            }
            else if (PlayerAnimator.runtimeAnimatorController.name == "Pink Player") {
                CharacterImage.sprite = PinkPlayerPortrait;

            }
            else if (PlayerAnimator.runtimeAnimatorController.name == "White Player") {
                CharacterImage.sprite = WhitePlayerPortrait;
            }
        }
    }
}
