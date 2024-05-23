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

    public GameObject HPBar; // 체력바 프리팹
    public Image HPMeterImage;
    public GameObject HPMeterInstance;

    public GameObject MonsterInfo; // 몬스터 정보 텍스트 프리팹
    public TMP_Text MonsterInfoText;
    public GameObject MonsterInfoInstance;
    PlayerStatus PlayerStatus;
    public int LevelDiff = 0;
    public bool BiggerThanPlayerLevel = false;
    public bool IsBossMonster = false;
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
        if (UIManager != null && HPBar != null && HPMeterInstance == null)
        {
            HPMeterInstance = Instantiate(HPBar, UIManager.transform); // 캔버스의 자식으로 할당
            HPMeterImage = HPMeterInstance.transform.GetChild(0).gameObject.GetComponentInChildren<Image>();
            UpdateHealthBar();
            HPMeterInstance.transform.SetAsFirstSibling();
        }
    }
    void UpdateHealthBar() // 체력바 갱신
    {
        if (HPMeterInstance != null) // 몬스터 체력바 위치 갱신
        {
            Vector3 newPosition = transform.position + Vector3.up * HPBarPos;
            HPMeterInstance.transform.position = newPosition;
        }

        if (HPMeterImage != null) // 몬스터 체력바 수치 갱신
        {
            HPMeterImage.fillAmount = MonsterCurrentHealth / MonsterMaxHealth;
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

    void UpdateMonsterInfo() {
        if (MonsterInfoInstance != null) {
            Vector3 newPosition = transform.position + Vector3.down * MonsterInfoPos;
            MonsterInfoInstance.transform.position = newPosition;
        }
    }

    public void SetLevelDiff(int PlayerLevel) {
        if (PlayerLevel >= MonsterLevel) { // 플레이어 LV > 몬스터 LV
            LevelDiff = PlayerLevel - MonsterLevel;
            if (LevelDiff >= 5) {
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
