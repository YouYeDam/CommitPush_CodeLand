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
        
        PlayerMaxEXP = PlayerMaxEXP + (int)Mathf.Floor(PlayerMaxEXP * 0.5f);
        if (PlayerCurrentEXP >= PlayerMaxEXP) {
            PlayerCurrentEXP = PlayerMaxEXP - 1;
        }
        Instantiate(LevelUpEffect, LevelUpSpot.position, transform.rotation);
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
        if (PlayerNameInfoInstance != null && Camera.main != null && UIManager != null) {
            Vector3 newPosition = transform.position + Vector3.down * PlayerNameInfoPos;
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(newPosition);

            RectTransform canvasRect = UIManager.GetComponent<RectTransform>();
            RectTransform nameTagRect = PlayerNameInfoInstance.GetComponent<RectTransform>();

            Vector2 localPoint;
            // 스크린 좌표를 캔버스의 로컬 좌표로 변환
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPosition, Camera.main, out localPoint)) {
                // 부드러운 이동을 위한 Lerp 사용
                Vector2 targetPosition = Vector2.Lerp(nameTagRect.anchoredPosition, localPoint, Time.deltaTime * 20);
                nameTagRect.anchoredPosition = targetPosition;
            }
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
