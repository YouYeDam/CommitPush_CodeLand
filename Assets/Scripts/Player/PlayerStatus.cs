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
    [SerializeField] GameObject LevelUpEffect;
    [SerializeField] Transform LevelUpSpot;

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
    public bool IsTonic = false;

    void Start() {
        PlayerCurrentHP = PlayerMaxHP;
        PlayerCurrentMP = PlayerMaxMP;
        UIManager = GameObject.Find("UIManager");
    }

    void LateUpdate() { // 플레이어 이름의 위치가 튀는 것을 방지하기 위해서
        UpdatePlayerNameInfo();
    }
    public void SetPlayerName(string NewName){ // 플레이어 이름 설정
            PlayerName = NewName;
    }

    public void GainEXP(int EXP) { // 경험치 획득
        PlayerCurrentEXP += EXP;
        if (PlayerCurrentEXP >= PlayerMaxEXP) {
            LevelUp();
        }
    }

    void LevelUp() { // 레벨업 기능
        PlayerLevel += 1;
        PlayerCurrentEXP -= PlayerMaxEXP; // 초과한 경험치 이전되도록
        PlayerMaxHP += 10;
        PlayerMaxMP += 10;
        PlayerCurrentHP = PlayerMaxHP;
        PlayerCurrentMP = PlayerMaxMP;
        LevelUpPoint += 3;

        // 레벨에 따라 요구 경험치 증가량 조정
        if (PlayerLevel <= 5) {
            PlayerMaxEXP = PlayerMaxEXP + (int)Mathf.Floor(PlayerMaxEXP * 0.8f);
        }
        else if (PlayerLevel <= 8) {
            PlayerMaxEXP = PlayerMaxEXP + (int)Mathf.Floor(PlayerMaxEXP * 0.65f);
        }
        else {
            PlayerMaxEXP = PlayerMaxEXP + (int)Mathf.Floor(PlayerMaxEXP * 0.55f);
        }

        if (PlayerCurrentEXP >= PlayerMaxEXP) { // 2단 레벨업 방지
            PlayerCurrentEXP = PlayerMaxEXP - 1;
        }

        Instantiate(LevelUpEffect, LevelUpSpot.position, transform.rotation); // 레벨업 이펙트

        GameObject[] Monsters = GameObject.FindGameObjectsWithTag("Monster"); // 몬스터와 플레이어 레벨차 재조정
        foreach (GameObject Monster in Monsters) {
            MonsterStatus MonsterStatus = Monster.GetComponent<MonsterStatus>();
            if (MonsterStatus != null) {
                MonsterStatus.SetLevelDiff(PlayerLevel);
            }
        }
    }

    public void DisplayPlayerNameInfo() { // 플레이어 이름 보이기
        if (UIManager != null && PlayerNameInfo != null && PlayerNameInfoInstance == null) {
            PlayerNameInfoInstance = Instantiate(PlayerNameInfo, UIManager.transform); // 캔버스의 자식으로 할당
            PlayerNameInfoText = PlayerNameInfoInstance.GetComponent<TMP_Text>();
            PlayerNameInfoText.text = PlayerName;
            PlayerNameInfoInstance.transform.SetAsFirstSibling();
        }
    }

    void UpdatePlayerNameInfo() { // 플레이어 이름 위치 갱신
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
        PlayerAP += 0.008f;
        LevelUpPoint -= 1;
    }

    public void CritUp() {
        PlayerCrit += 0.008f;
        LevelUpPoint -= 1;
    }

    public void AutoHeal(float Bonus) { // 체력 자동회복
        if (PlayerCurrentHP != PlayerMaxHP) {
            PlayerCurrentHP += Mathf.RoundToInt(PlayerMaxHP * 0.03f);
            if (PlayerCurrentHP > PlayerMaxHP) {
                PlayerCurrentHP = PlayerMaxHP;
            }
        }

        if (PlayerCurrentMP != PlayerMaxMP) {
            PlayerCurrentMP += Mathf.RoundToInt(PlayerMaxMP * 0.05f);
            if (PlayerCurrentMP > PlayerMaxMP) {
                PlayerCurrentMP = PlayerMaxMP;
            }
        }
    }
}
