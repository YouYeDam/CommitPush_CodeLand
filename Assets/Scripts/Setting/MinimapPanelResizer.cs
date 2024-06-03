using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MinimapPanelResizer : MonoBehaviour
{
    public RectTransform miniMapPanel;
    public TextMeshProUGUI mapNameTMP;
    public Camera miniMapCamera;
    public Canvas canvas;
    public float horizontalPadding = 0f;
    public float bottomPadding = 130f;

    // Start is called before the first frame update
    void Start()
    {
        UpdateMiniMapPanel();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMiniMapPanel();
    
    }

    void UpdateMiniMapPanel()
    {
        Rect viewportRect = miniMapCamera.rect;

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        float panelWidth = viewportRect.width * canvasRect.rect.width + horizontalPadding * 2;
        float panelHeight = viewportRect.height * canvasRect.rect.height + bottomPadding;
        float panelPosX = viewportRect.x * canvasRect.rect.width - canvasRect.rect.width / 2 + panelWidth / 2 - horizontalPadding;
        float panelPosY = viewportRect.y * canvasRect.rect.height - canvasRect.rect.height / 2 + panelHeight / 2 - bottomPadding / 2;

        miniMapPanel.sizeDelta = new Vector2(panelWidth, panelHeight);
        miniMapPanel.anchoredPosition = new Vector2(panelPosX, panelPosY);

        RectTransform mapNameRect = mapNameTMP.GetComponent<RectTransform>();
        mapNameRect.anchorMin = new Vector2(0, 0);
        mapNameRect.anchorMax = new Vector2(1, 0);
        mapNameRect.pivot = new Vector2(0.5f, 0);
        mapNameRect.sizeDelta = new Vector2(0, bottomPadding);
        mapNameRect.anchoredPosition = new Vector2(5f, -bottomPadding / 2 - 5f);

        mapNameTMP.fontSize = 25;

    }
}
