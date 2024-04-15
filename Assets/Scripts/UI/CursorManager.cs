using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D CursorTexture; // 변경할 마우스 커서 이미지
    public Vector2 HotSpot = Vector2.zero; // 마우스 커서 이미지의 중심 좌표

    void Start()
    {
        // 마우스 커서 이미지 변경
        Cursor.SetCursor(CursorTexture, HotSpot, CursorMode.Auto);
    }
}