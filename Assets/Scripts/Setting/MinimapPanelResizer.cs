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
    public float bottomPadding = 40f;

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
        // 미니맵 카메라의 Viewport Rect 가져오기
        Rect viewportRect = miniMapCamera.rect;

        // 캔버스의 실제 크기 가져오기
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        // Viewport Rect를 기준으로 패널의 크기와 위치 계산
        float panelWidth = viewportRect.width * canvasRect.rect.width + horizontalPadding * 2;
        float panelHeight = viewportRect.height * canvasRect.rect.height + bottomPadding;
        float panelPosX = viewportRect.x * canvasRect.rect.width - canvasRect.rect.width / 2 + panelWidth / 2 - horizontalPadding;
        float panelPosY = viewportRect.y * canvasRect.rect.height - canvasRect.rect.height / 2 + panelHeight / 2 - bottomPadding / 2;

        // 패널의 크기 설정 (여유 공간 포함)
        miniMapPanel.sizeDelta = new Vector2(panelWidth, panelHeight);
        miniMapPanel.anchoredPosition = new Vector2(panelPosX, panelPosY);

        // 텍스트 요소의 크기와 위치 설정 (패널 아래쪽 여유 공간에 위치)
        RectTransform mapNameRect = mapNameTMP.GetComponent<RectTransform>();
        mapNameRect.anchorMin = new Vector2(0, 0);
        mapNameRect.anchorMax = new Vector2(1, 0);
        mapNameRect.pivot = new Vector2(0.5f, 0);
        mapNameRect.sizeDelta = new Vector2(0, bottomPadding);
        mapNameRect.anchoredPosition = new Vector2(5f, -bottomPadding / 2 - 5f);

    }
}
