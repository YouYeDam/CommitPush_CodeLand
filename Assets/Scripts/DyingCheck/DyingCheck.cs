using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class DyingCheck : MonoBehaviour
{
    public GameObject DyingCheckBase;
    GameObject ReviveSceneNameObject;
    PlayerStatus PlayerStatus;
    ReviveSceneName ReviveSceneName;
    UIManager UIManager;
    void Start() {
        PlayerStatus = FindObjectOfType<PlayerStatus>();
        UIManager = FindObjectOfType<UIManager>();
    }

    public void ActivateDyingCheck() {
        DyingCheckBase.SetActive(true);
    }

    public void RevivePlayer() {
        PlayerStatus.PlayerCurrentHP = PlayerStatus.PlayerMaxHP / 2;
        PlayerStatus.PlayerCurrentMP = PlayerStatus.PlayerMaxMP / 2;
        PlayerStatus.PlayerCurrentEXP = Mathf.RoundToInt(PlayerStatus.PlayerCurrentEXP * 0.8f);
        UIManager.DestroyAllTempInfo();
        
        ReviveSceneNameObject = GameObject.Find("ReviveSceneName");
        if (ReviveSceneNameObject != null) {
            ReviveSceneName = ReviveSceneNameObject.GetComponent<ReviveSceneName>();
            string ReviveSceneNameInfo = ReviveSceneName.ReviveSceneNameInfo;
            DyingCheckBase.SetActive(false);
            SceneManager.LoadScene(ReviveSceneNameInfo);
        }
    }
}
