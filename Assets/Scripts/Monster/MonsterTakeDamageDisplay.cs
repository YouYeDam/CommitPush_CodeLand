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

    public void DisplayDamageBar(int Damage, bool IsCrit) { // 데미지바 보이기
        if (UIManager != null && DamageBar != null) {
            // 이전 인스턴스가 있으면 삭제(데미지바가 겹쳐서 확인이 불가능한 경우가 없도록)
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

            StartCoroutine(FadeOutAndDestroy(DamageText, DamageBarInstance, Delay)); // 일정 시간 후 데미지바 삭제 호출
        }
    }

    void UpdateDamageBarPosition() { // 데미지바가 몬스터를 따라다니도록 위치 갱신
        if (DamageBarInstance != null) {
            Vector3 newPosition = transform.position + Vector3.up * DamageBarPos;
            DamageBarInstance.transform.position = newPosition;
        }
    }

    IEnumerator FadeOutAndDestroy(TMP_Text DamageText, GameObject DamageBarInstance, float Delay) { // 데미지바 점점 소멸하다 삭제
        float ElapsedTime = 0;
        Color OriginalColor = DamageText.color;

        while (ElapsedTime < Delay) { // 데미지바가 점점 소멸하다 삭제될 수 있도록 알파값 조정
            float Alpha = Mathf.Lerp(1f, 0f, ElapsedTime / Delay);
            DamageText.color = new Color(OriginalColor.r, OriginalColor.g, OriginalColor.b, Alpha);
            ElapsedTime += Time.deltaTime;
            yield return null;
        }

        DamageText.color = new Color(OriginalColor.r, OriginalColor.g, OriginalColor.b, 0);
        Destroy(DamageBarInstance);
    }
}
