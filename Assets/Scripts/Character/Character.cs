using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Character : MonoBehaviour
{
    GameObject Player;
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
    public TMP_Text SPInfo;
    public TMP_Text CritInfo;
    public Image CharacterImage;
    
    PlayerStatus PlayerStatus;
    Animator PlayerAnimator;

    void Start() {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerStatus = Player.GetComponent<PlayerStatus>();
        PlayerAnimator = Player.GetComponent<Animator>();
    }
    
    void Update() {
        NameInfo.text = "이름:" + PlayerStatus.PlayerName;
        ClassInfo.text = "클래스:" + PlayerStatus.PlayerClass;
        LevelInfo.text = "코딩력:" + PlayerStatus.PlayerLevel + "LV";
        HPInfo.text = "체력:" + PlayerStatus.PlayerMaxHP + "HP";
        MPInfo.text = "정신력:" + PlayerStatus.PlayerMaxMP + "MP";
        ATKInfo.text = "공격력:" + PlayerStatus.PlayerATK + "ATK";
        DEFInfo.text = "방어력:" + PlayerStatus.PlayerDEF + "DEF";
        SPInfo.text = "지구력:" + PlayerStatus.PlayerMaxSP + "SP";
        CritInfo.text = "치명타:" + PlayerStatus.PlayerCrit + "%";
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
