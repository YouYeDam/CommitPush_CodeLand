using TMPro;
using UnityEngine;
public class PlayerStatus : MonoBehaviour
{
    public string PlayerName; // 플레이어 이름
    public string PlayerClass = "개발자";
    public GameObject UIManager;
    public GameObject PlayerNameInfo; // 플레이어 이름 텍스트 프리팹
    public TMP_Text PlayerNameInfoText;
    public GameObject PlayerNameInfoInstance;
    public float PlayerNameInfoPos = 0.5f;
    [SerializeField] public int PlayerLevel = 1;
    [SerializeField] public int PlayerMaxHP = 100;
    [SerializeField] public int PlayerCurrentHP;
    [SerializeField] public int PlayerMaxMP = 100;
    [SerializeField] public int PlayerCurrentMP;
    [SerializeField] public int PlayerMaxEXP = 10; 
    [SerializeField] public int PlayerCurrentEXP = 0;
    [SerializeField] public int PlayerATK = 0;
    [SerializeField] public int PlayerDEF = 0;
    [SerializeField] public float PlayerAP = 0f;
    [SerializeField] public float PlayerCrit = 0.05f;
    public int LevelUpPoint = 0;

    void Start() {
        PlayerCurrentHP = PlayerMaxHP;
        PlayerCurrentMP = PlayerMaxMP;
        UIManager = GameObject.Find("UIManager");
    }
    void Update() {
        UpdatePlayerNameInfo();
    }
    public void SetPlayerName(string NewName){
            PlayerName = NewName;
    }

    public void GainEXP(int EXP) {
        PlayerCurrentEXP += EXP;
        if (PlayerCurrentEXP >= PlayerMaxEXP) {
            LevelUp();
            PlayerMaxEXP = PlayerMaxEXP + (int)Mathf.Floor(PlayerMaxEXP * 0.5f);
        }
    }

    void LevelUp() {
        PlayerLevel += 1;
        PlayerCurrentEXP -= PlayerMaxEXP;
        PlayerMaxHP += 10;
        PlayerMaxMP += 10;
        PlayerCurrentHP = PlayerMaxHP;
        PlayerCurrentMP = PlayerMaxMP;
        LevelUpPoint += 3;
    }

    public void DisplayPlayerNameInfo() { // 캐릭터 이름 보이기
        if (UIManager != null && PlayerNameInfo != null && PlayerNameInfoInstance == null) {
            PlayerNameInfoInstance = Instantiate(PlayerNameInfo, UIManager.transform); // 캔버스의 자식으로 할당
            PlayerNameInfoText = PlayerNameInfoInstance.GetComponent<TMP_Text>();
            PlayerNameInfoText.text = PlayerName;
            PlayerNameInfoInstance.transform.SetAsFirstSibling();
        }
    }

    void UpdatePlayerNameInfo() {
        if (PlayerNameInfoInstance != null) {
            Vector3 newPosition = transform.position + Vector3.down * PlayerNameInfoPos;
            PlayerNameInfoInstance.transform.position = newPosition;
        }
    }

    public void HPUp() {
        PlayerMaxHP += 10;
        PlayerCurrentHP += 10;
        LevelUpPoint -= 1;
    }

    public void MPUp() {
        PlayerMaxMP += 10;
        PlayerCurrentMP += 10;
        LevelUpPoint -= 1;
    }

    public void ATKUp() {
        PlayerATK += 1;
        LevelUpPoint -= 1;
    }

    public void DEFUp() {
        PlayerDEF += 1;
        LevelUpPoint -= 1;
    }

    public void APUp() {
        PlayerAP += 0.02f;
        LevelUpPoint -= 1;
    }

    public void CritUp() {
        PlayerCrit += 0.008f;
        LevelUpPoint -= 1;
    }
}
