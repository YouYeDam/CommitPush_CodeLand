using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerTakeDamageDisplay : MonoBehaviour
{
    public float DamageBarPos = 0.5f;
    public GameObject UIManager;
    public GameObject DamageBar;
    public GameObject DamageBarInstance;
    PlayerStatus PlayerStatus;
    float Delay = 0.75f;

    void Start() {
        UIManager = GameObject.Find("UIManager");
        PlayerStatus = GetComponent<PlayerStatus>();
    }

    void Update() {
        UpdateDamageBarPosition();
    }

    public void DisplayDamageBar(int Damage) {
        if (UIManager != null && DamageBar != null) {
            // 이전 인스턴스가 있으면 삭제 (데미지바가 겹쳐서 확인이 불가능한 경우 방지)
            if (DamageBarInstance != null) {
                Destroy(DamageBarInstance);
            }

            DamageBarInstance = Instantiate(DamageBar, UIManager.transform);
            TMP_Text DamageText = DamageBarInstance.GetComponent<TMP_Text>();
            DamageText.text = Damage.ToString();
            DamageBarInstance.transform.SetAsFirstSibling();

            StartCoroutine(FadeOutAndDestroy(DamageText, DamageBarInstance, Delay));
        }
    }

    void UpdateDamageBarPosition() { // 데미지바가 플레이어 위치를 따라가도록
        if (DamageBarInstance != null) {
            Vector3 newPosition = transform.position + Vector3.up * DamageBarPos;
            DamageBarInstance.transform.position = newPosition;
        }
    }

    IEnumerator FadeOutAndDestroy(TMP_Text DamageText, GameObject DamageBarInstance, float Delay) { // 데미지바가 점점 소멸하다가 없어지도록 코루틴
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
