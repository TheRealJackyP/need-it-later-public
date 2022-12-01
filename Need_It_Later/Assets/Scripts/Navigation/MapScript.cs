using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Tilemap floorTilemap;
    public List<Vector2> floorPositions = new List<Vector2>();
    void Start()
    {
        foreach (var pos in floorTilemap.cellBounds.allPositionsWithin)
        {
            if (floorTilemap.HasTile(pos))
            {
                floorPositions.Add(new Vector2Int(pos.x, pos.y));
            }
        }

    }
    
    public Vector2 GetRandomFloorPosition()
    {
        return floorPositions[Random.Range(0, floorPositions.Count)] + new Vector2(.5f, .5f);
    }
}
