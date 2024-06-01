using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapToggle : MonoBehaviour
{
    public GameObject miniMapCamera;
    public GameObject miniMapCanvas;

    void Start()
    {
        if (PlayerManager.Instance != null)
        {
            UpdateMiniMapState(PlayerManager.Instance.isMiniMapActive);
        }
        else
        {
            Debug.LogError("PlayerManager instance is not found.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (PlayerManager.Instance != null)
            {
                bool currentState = !PlayerManager.Instance.isMiniMapActive;
                PlayerManager.Instance.SetMiniMapState(currentState);
                UpdateMiniMapState(currentState);
            }
        }
    }

    void UpdateMiniMapState(bool state)
    {
        miniMapCamera.SetActive(state);
        miniMapCanvas.SetActive(state);
    }
}

