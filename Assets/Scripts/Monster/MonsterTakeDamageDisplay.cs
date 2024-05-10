using System.Collections;
using UnityEngine;
using TMPro;

public class MonsterTakeDamageDisplay : MonoBehaviour
{
    public float DamageBarPos;
    public GameObject UIManager;
    public GameObject DamageBar; // 데미지바 프리팹
    public GameObject DamageBarInstance; // 데미지바 인스턴스
    MonsterStatus MonsterStatus;
    float Delay = 0.75f;

    void Start() {
        UIManager = GameObject.Find("UIManager");
        MonsterStatus = GetComponent<MonsterStatus>();
        DamageBarPos = MonsterStatus.HPBarPos + 0.5f;
    }

    void Update() {
        UpdateDamageBarPosition();
    }

    public void DisplayDamageBar(int Damage, bool IsCrit) {
        if (UIManager != null && DamageBar != null) {
            // 이전 인스턴스가 있으면 삭제
            if (DamageBarInstance != null) {
                Destroy(DamageBarInstance);
            }

            DamageBarInstance = Instantiate(DamageBar, UIManager.transform);
            TMP_Text DamageText = DamageBarInstance.GetComponent<TMP_Text>();
            DamageText.text = Damage.ToString();
            DamageBarInstance.transform.SetAsFirstSibling();

            if (IsCrit) {
                DamageText.color = Color.red;
            }

            StartCoroutine(FadeOutAndDestroy(DamageText, DamageBarInstance, Delay));
        }
    }

    void UpdateDamageBarPosition() {
        if (DamageBarInstance != null) {
            Vector3 newPosition = transform.position + Vector3.up * DamageBarPos;
            DamageBarInstance.transform.position = newPosition;
        }
    }

    IEnumerator FadeOutAndDestroy(TMP_Text DamageText, GameObject DamageBarInstance, float Delay) {
        float ElapsedTime = 0;
        Color OriginalColor = DamageText.color;

        while (ElapsedTime < Delay) {
            float Alpha = Mathf.Lerp(1f, 0f, ElapsedTime / Delay);
            DamageText.color = new Color(OriginalColor.r, OriginalColor.g, OriginalColor.b, Alpha);
            ElapsedTime += Time.deltaTime;
            yield return null;
        }

        DamageText.color = new Color(OriginalColor.r, OriginalColor.g, OriginalColor.b, 0);
        Destroy(DamageBarInstance);
    }
}
