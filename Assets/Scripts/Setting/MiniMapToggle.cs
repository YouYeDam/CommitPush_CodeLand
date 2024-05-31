using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapToggle : MonoBehaviour
{
    public GameObject miniMapCamera; 
    public GameObject miniMapCanvas; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            bool isActive = miniMapCamera.activeSelf;
            miniMapCamera.SetActive(!isActive);
            miniMapCanvas.SetActive(!isActive);
        }
    }
}

