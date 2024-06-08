using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEquipment : MonoBehaviour
{
    PlayerStatus PlayerStatus;
    private Coroutine PythonEffectCoroutine;

    void Start() {
        PlayerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
    }

    public void SpecialEquipmentEffect(string EquipmentType, string ItemName) {
        switch (EquipmentType) {
            case "펫":
                PetSpecialEffect(ItemName);
                break;
        }
    }

    void PetSpecialEffect(string ItemName) {
        switch (ItemName) {
            case "파이썬":
                PythonEffect(true);
                break;
        }
    }

    public void DeSpecialEquipmentEffect(string EquipmentType, string ItemName) {
        switch (EquipmentType) {
            case "펫":
                DePetSpecialEffect(ItemName);
                break;
        }
    }

    void DePetSpecialEffect(string ItemName) {
        switch (ItemName) {
            case "파이썬":
                PythonEffect(false);
                break;
        }
    }

    void PythonEffect(bool ActivateEffect) {
        if (ActivateEffect) {
            if (PythonEffectCoroutine == null) {
                PythonEffectCoroutine = StartCoroutine(PythonAutoHeal());
            }
        } else {
            if (PythonEffectCoroutine != null) {
                StopCoroutine(PythonEffectCoroutine);
                PythonEffectCoroutine = null;
            }
        }
    }

    private IEnumerator PythonAutoHeal() {
        while (true) {
            yield return new WaitForSeconds(10f);
            AutoHeal();
        }
    }

    private void AutoHeal() {
        if (PlayerStatus.PlayerCurrentHP < PlayerStatus.PlayerMaxHP) {
            PlayerStatus.PlayerCurrentHP += Mathf.RoundToInt(PlayerStatus.PlayerMaxHP * 0.05f);
            if (PlayerStatus.PlayerCurrentHP > PlayerStatus.PlayerMaxHP) {
                PlayerStatus.PlayerCurrentHP = PlayerStatus.PlayerMaxHP;
            }
        }

        if (PlayerStatus.PlayerCurrentMP < PlayerStatus.PlayerMaxMP) {
            PlayerStatus.PlayerCurrentMP += Mathf.RoundToInt(PlayerStatus.PlayerMaxMP * 0.05f);
            if (PlayerStatus.PlayerCurrentMP > PlayerStatus.PlayerMaxMP) {
                PlayerStatus.PlayerCurrentMP = PlayerStatus.PlayerMaxMP;
            }
        }
    }
}
