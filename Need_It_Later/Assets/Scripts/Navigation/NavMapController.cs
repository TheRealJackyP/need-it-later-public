using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

namespace Navigation
{
    public class NavMapController : MonoBehaviour
    {
        [SerializeField] internal NavMeshSurface surface2D;
        public Tilemap floorTilemap;
        public Tilemap wallTilemap;

        public List<Vector2Int> floorPositions = new List<Vector2Int>();
        
        private GeneratorManager _generatorManager;
        //[SerializeField] private GameObject playerPrefab;

        private void Start()
        {
            
            NewMap();
            var p = FindObjectOfType<PlayerAim>().gameObject;
            var pos = GetRandomFloorPosition();
            p.transform.position = new Vector2(pos.x, pos.y);
            var aim = p.GetComponent<PlayerAim>();
            aim.AimReticule = GameObject.Find("AimReticule").gameObject.transform;
            aim.TargetCamera = Camera.main;

        }
        
        public void NewMap()
        {
            GenerateMap();
            BuildNavMesh();
        }
        private void BuildNavMesh()
        {
            surface2D.BuildNavMesh();
        }
        
        internal void GenerateMap()
        {
            /*floorTilemap.ClearAllTiles();
            wallTilemap.ClearAllTiles();*/
            floorPositions.Clear();
            
            _generatorManager = FindObjectOfType<GeneratorManager>();
            _generatorManager.ClearAllMaps();
            _generatorManager.GenerateNewMap("BSP");

            foreach (var pos in floorTilemap.cellBounds.allPositionsWithin)
            {
                if (floorTilemap.HasTile(pos))
                {
                    floorPositions.Add(new Vector2Int(pos.x, pos.y));
                }
            }
        }
        
        public Vector2Int GetRandomFloorPosition()
        {
            return floorPositions[Random.Range(0, floorPositions.Count)];
        }
    
    }
}
