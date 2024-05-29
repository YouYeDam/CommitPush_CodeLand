using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerLevelUpController : MonoBehaviour
{
    GameObject HPUPButtonObject;
    GameObject MPUPButtonObject;
    GameObject ATKUPButtonObject;
    GameObject DEFUPButtonObject;
    GameObject APUPButtonObject;
    GameObject CritUPButtonObject;
    Button HPUPButton;
    Button MPUPButton;
    Button ATKUPButton;
    Button DEFUPButton;
    Button APUPButton;
    Button CritUPButton;
    PlayerStatus PlayerStatus;

    GameObject UIManager;
    GameObject Character;
    void Start() {
        PlayerStatus = GetComponent<PlayerStatus>();
        UIManager = GameObject.Find("UIManager");
        Character = UIManager.transform.GetChild(2).gameObject;
    }

    void Update() {
        if (Character.activeSelf) {
            CheckLevelUpPoint();
        }
    }
    public void ConnectButton() {
        HPUPButtonObject = GameObject.Find("HP UP Button");
        HPUPButton = HPUPButtonObject.GetComponent<Button>();
        HPUPButton.onClick.AddListener(PlayerStatus.HPUp);

        MPUPButtonObject = GameObject.Find("MP UP Button");
        MPUPButton = MPUPButtonObject.GetComponent<Button>();
        MPUPButton.onClick.AddListener(PlayerStatus.MPUp);

        ATKUPButtonObject = GameObject.Find("ATK UP Button");
        ATKUPButton = ATKUPButtonObject.GetComponent<Button>();
        ATKUPButton.onClick.AddListener(PlayerStatus.ATKUp);

        DEFUPButtonObject = GameObject.Find("DEF UP Button");
        DEFUPButton = DEFUPButtonObject.GetComponent<Button>();
        DEFUPButton.onClick.AddListener(PlayerStatus.DEFUp);

        APUPButtonObject = GameObject.Find("AP UP Button");
        APUPButton = APUPButtonObject.GetComponent<Button>();
        APUPButton.onClick.AddListener(PlayerStatus.APUp);

        CritUPButtonObject = GameObject.Find("Crit UP Button");
        CritUPButton = CritUPButtonObject.GetComponent<Button>();
        CritUPButton.onClick.AddListener(PlayerStatus.CritUp);
    }

    public void CheckLevelUpPoint() {
        if (PlayerStatus.LevelUpPoint >= 1) {
            HPUPButton.interactable = true;
            MPUPButton.interactable = true;
            ATKUPButton.interactable = true;
            DEFUPButton.interactable = true;
            APUPButton.interactable = true;
            CritUPButton.interactable = true;
        }
        else {
            HPUPButton.interactable = false;
            MPUPButton.interactable = false;
            ATKUPButton.interactable = false;
            DEFUPButton.interactable = false;
            APUPButton.interactable = false;
            CritUPButton.interactable = false;
        }
    }
}
