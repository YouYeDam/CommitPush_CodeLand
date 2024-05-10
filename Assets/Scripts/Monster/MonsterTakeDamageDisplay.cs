using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonsterTakeDamageDisplay : MonoBehaviour
{
    public float DamageBarPos;
    public GameObject UIManager;
    public GameObject DamageBar; // 데미지바 프리팹
    public List<GameObject> DamageBarInstances; // 생성된 모든 데미지바 인스턴스를 저장하는 리스트
    MonsterStatus MonsterStatus;
    float Delay = 0.5f;
    void Start() {
        UIManager = GameObject.Find("UIManager");
        DamageBarInstances = new List<GameObject>();
        MonsterStatus = GetComponent<MonsterStatus>();
        DamageBarPos = MonsterStatus.HPBarPos + 0.2f;
    }

    void Update() {
        UpdateDamageBarPositions();
    }

    public void DisplayDamageBar(int Damage, bool IsCrit) {
        if (UIManager != null && DamageBar != null) {
            GameObject NewDamageBarInstance = Instantiate(DamageBar, UIManager.transform); // 캔버스의 자식으로 할당
            TMP_Text DamageText = NewDamageBarInstance.GetComponent<TMP_Text>();
            DamageText.text = Damage.ToString();
            NewDamageBarInstance.transform.SetAsFirstSibling();

            if (IsCrit) {
                DamageText.color = Color.red;
            }

            DamageBarInstances.Add(NewDamageBarInstance);
            StartCoroutine(DestroyAfterDelay(NewDamageBarInstance, Delay));
        }
    }

    void UpdateDamageBarPositions() {
        foreach (var instance in DamageBarInstances) {
            Vector3 newPosition = transform.position + Vector3.up * DamageBarPos;
            instance.transform.position = newPosition;
        }
    }

    IEnumerator DestroyAfterDelay(GameObject damageBarInstance, float Delay) {
        yield return new WaitForSeconds(Delay);
        DamageBarInstances.Remove(damageBarInstance);
        Destroy(damageBarInstance);
    }
}
