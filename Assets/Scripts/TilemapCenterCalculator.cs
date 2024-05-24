using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NewBehaviourScript : MonoBehaviour
{
    public Tilemap platformsTilemap;
    // Start is called before the first frame update
    void Start()
    {
        if (platformsTilemap != null)
        {
            // 타일맵의 bounds를 가져옴
            BoundsInt bounds = platformsTilemap.cellBounds;
            
            // 중앙 좌표 계산
            Vector3 min = bounds.min;
            Vector3 max = bounds.max;
            Vector3 center = (min + max) / 2f;

            // 월드 좌표로 변환
            Vector3 worldCenter = platformsTilemap.CellToWorld(new Vector3Int(Mathf.FloorToInt(center.x), Mathf.FloorToInt(center.y), Mathf.FloorToInt(center.z)));

            Debug.Log("플랫폼 타일맵 중앙 좌표: " + worldCenter);
        }
        else
        {
            Debug.LogError("Platforms Tilemap이 설정되지 않았습니다.");
        }
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
