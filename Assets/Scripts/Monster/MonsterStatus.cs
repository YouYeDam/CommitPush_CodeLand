using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterStatus : MonoBehaviour
{
    [SerializeField] public string MonsterName;
    [SerializeField] public int MonsterLevel = 1;
    [SerializeField] public float MonsterMaxHealth;
    [SerializeField] public float MonsterCurrentHealth;
    [SerializeField] public int MonsterDamage = 10;
    [SerializeField] public int MonsterEXP = 1;
    
    public float HPBarPos = 0.5f;
    public float MonsterInfoPos = 0.5f;
    public GameObject UIManager;

    public GameObject HPBar;
    public Image HPMeterImage;
    public TMP_Text HPPercentText;
    public GameObject HPMeterInstance;

    public GameObject MonsterInfo; // 몬스터 정보 텍스트 프리팹
    public TMP_Text MonsterInfoText;
    public GameObject MonsterInfoInstance;
    PlayerStatus PlayerStatus;
    public int LevelDiff = 0; // 몬스터와 플레이어 간 레벨 차
    public bool BiggerThanPlayerLevel = false; // 몬스터 LV > 플레이어 LV

    public bool IsBossMonster = false;
    public bool IsSummoningMonster = false;
    public bool IsSummonedMonster = false;

    void Start() {
        MonsterCurrentHealth = MonsterMaxHealth;
        UIManager = GameObject.Find("UIManager");
        PlayerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
        SetLevelDiff(PlayerStatus.PlayerLevel);
    }

    void Update() {
        UpdateHealthBar();
        UpdateMonsterInfo();
    }

    public void DisplayHPMeter() { // 몬스터 체력바 보이기
        if (UIManager != null && HPBar != null && HPMeterInstance == null) {
            HPMeterInstance = Instantiate(HPBar, UIManager.transform); // 캔버스의 자식으로 할당
            HPMeterImage = HPMeterInstance.transform.GetChild(0).gameObject.GetComponentInChildren<Image>();

            if (IsBossMonster) { // 보스몬스터라면 전용 체력바 전시
                HPPercentText = HPMeterInstance.transform.GetChild(1).gameObject.GetComponentInChildren<TMP_Text>();
            }

            UpdateHealthBar();
            HPMeterInstance.transform.SetAsFirstSibling();
        }
    }
    void UpdateHealthBar() { // 체력바 갱신
        if (HPMeterInstance != null) // 몬스터 체력바 위치 갱신
        {
            Vector3 newPosition = transform.position + Vector3.up * HPBarPos;
            HPMeterInstance.transform.position = newPosition;
        }

        if (HPMeterImage != null) // 몬스터 체력바 수치 갱신
        {
            HPMeterImage.fillAmount = MonsterCurrentHealth / MonsterMaxHealth;
        }

        if (HPPercentText != null) // 몬스터 체력바 퍼센트 표시 갱신
        {
            float HealthPercent = (MonsterCurrentHealth / MonsterMaxHealth) * 100f;
            HPPercentText.text = HealthPercent.ToString("F1") + "%";
        }
    }
    
    public void DisplayMonsterInfo() { // 몬스터 정보(이름, 레벨) 보이기
        if (UIManager != null && MonsterInfo != null && MonsterInfoInstance == null) {
            MonsterInfoInstance = Instantiate(MonsterInfo, UIManager.transform); // 캔버스의 자식으로 할당
            MonsterInfoText = MonsterInfoInstance.GetComponent<TMP_Text>();
            MonsterInfoText.text = "LV." + MonsterLevel + " " + MonsterName;
            MonsterInfoInstance.transform.SetAsFirstSibling();
        }
    }

    void UpdateMonsterInfo() { // 몬스터 정보 위치 갱신
        if (MonsterInfoInstance != null) {
            Vector3 newPosition = transform.position + Vector3.down * MonsterInfoPos;
            MonsterInfoInstance.transform.position = newPosition;
        }
    }

    public void SetLevelDiff(int PlayerLevel) { // 몬스터와 플레이어 간 레벨 차 세팅
        if (PlayerLevel >= MonsterLevel) { // 플레이어 LV > 몬스터 LV
            LevelDiff = PlayerLevel - MonsterLevel;
            if (LevelDiff >= 5) { // 최대 레벨 차는 5레벨이 넘어가지 못하도록(과도하게 차이가 나는 것을 방지)
                LevelDiff = 5;
            }
            LevelDiff *= 5;
            BiggerThanPlayerLevel = false;
        }
        else { // 몬스터 LV > 플레이어 LV
            LevelDiff = MonsterLevel - PlayerLevel;
            if (LevelDiff >= 5) {
                LevelDiff = 5;
            }
            LevelDiff *= 10;
            BiggerThanPlayerLevel = true;
        }
    }
}
