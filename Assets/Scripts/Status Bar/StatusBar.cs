using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    public Image HPMeter;
    public Image MPMeter;
    public Image EXPMeter;
    public TMP_Text HPDisplay;
    public TMP_Text MPDisplay;
    public TMP_Text EXPDisplay;
    public TMP_Text PlayerLV;
    public TMP_Text PlayerName;
    public PlayerStatus PlayerStatus;
    void Start(){
        PlayerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
    }

void Update() {
    if (PlayerStatus.PlayerCurrentHP > 0)
        HPMeter.fillAmount = (float)PlayerStatus.PlayerCurrentHP / PlayerStatus.PlayerMaxHP;
    else
        HPMeter.fillAmount = 0;

    if (PlayerStatus.PlayerCurrentMP > 0)
        MPMeter.fillAmount = (float)PlayerStatus.PlayerCurrentMP / PlayerStatus.PlayerMaxMP;
    else
        MPMeter.fillAmount = 0;

    if (PlayerStatus.PlayerCurrentEXP > 0)
        EXPMeter.fillAmount = (float)PlayerStatus.PlayerCurrentEXP / PlayerStatus.PlayerMaxEXP;
    else
        EXPMeter.fillAmount = 0;

    HPDisplay.text = PlayerStatus.PlayerCurrentHP + " / " + PlayerStatus.PlayerMaxHP;
    MPDisplay.text = PlayerStatus.PlayerCurrentMP + " / " + PlayerStatus.PlayerMaxMP;
    EXPDisplay.text = PlayerStatus.PlayerCurrentEXP + " / " + PlayerStatus.PlayerMaxEXP;
    PlayerLV.text = "LV." + PlayerStatus.PlayerLevel + " " + PlayerStatus.PlayerClass;
    PlayerName.text = PlayerStatus.PlayerName;
}

}
