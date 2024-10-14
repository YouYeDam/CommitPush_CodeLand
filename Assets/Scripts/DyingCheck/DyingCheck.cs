using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class DyingCheck : MonoBehaviour
{
    public GameObject DyingCheckBase;
    GameObject ReviveSceneNameObject;
    PlayerStatus PlayerStatus;
    ReviveSceneName ReviveSceneName;
    UIManager UIManager;
    public TextMeshProUGUI ButtonText;

    void Start() {
        PlayerStatus = FindObjectOfType<PlayerStatus>();
        UIManager = FindObjectOfType<UIManager>();
    }

    public void ActivateDyingCheck() { // 죽음 확인 창 활성화
        DyingCheckBase.SetActive(true);
    }

    public void RevivePlayer() { // 확인 버튼 클릭 시 마을에서 부활
        PlayerStatus.PlayerCurrentHP = PlayerStatus.PlayerMaxHP / 2;
        PlayerStatus.PlayerCurrentMP = PlayerStatus.PlayerMaxMP / 2;
        PlayerStatus.PlayerCurrentEXP = PlayerStatus.PlayerCurrentEXP - Mathf.RoundToInt(PlayerStatus.PlayerMaxEXP * 0.2f);
        if (PlayerStatus.PlayerCurrentEXP < 0) {
            PlayerStatus.PlayerCurrentEXP = 0;
        }
        UIManager.DestroyAllTempInfo();
        
        ReviveSceneNameObject = GameObject.Find("ReviveSceneName"); 

        if (ReviveSceneNameObject != null) { // 지정된 부활 장소(마을)에서 부활
            ReviveSceneName = ReviveSceneNameObject.GetComponent<ReviveSceneName>();
            string ReviveSceneNameInfo = ReviveSceneName.ReviveSceneNameInfo;
            ButtonText.color = Color.white;
            DyingCheckBase.SetActive(false);
            SceneManager.LoadScene(ReviveSceneNameInfo);
        }
    }
}
