using System.Collections.Generic;
using Health;
using UnityEngine;

namespace Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        [Foldout("Parameters", foldEverything = true, styled = true, readOnly = false)]
        [SerializeField]
        private int _maxEnemiesInScene;

        [Tooltip("Cooldown between spawns in milliseconds.")] [SerializeField]
        private int _spawnCooldown;

        public List<GameObject> ActiveEnemies = new();

        [Foldout("References", foldEverything = true, styled = true, readOnly = false)]
        [SerializeField]
        public GameObject _enemyPrefab;

        [SerializeField] private Transform _playerTransform;
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _enemiesContainer;

        [Foldout("Debug", foldEverything = true, styled = true, readOnly = false)]
        [SerializeField]
        private int _debugSpawnWaveCount;

        #region Spawning

        public void SpawnEnemy(Vector2 position)
        {
            var enemy = Instantiate(
                _enemyPrefab,
                _playerTransform.position + (Vector3) position,
                Quaternion.identity,
                _enemiesContainer);
            enemy.GetComponent<Enemy>().Init(_playerTransform);
            ActiveEnemies.Add(enemy);
            enemy.GetComponent<EntityHealth>()
                .OnEntityDie.AddListener(
                    (targetHealth, targetObject) => ActiveEnemies.Remove(targetObject));
        }

        public void SpawnCircle(float r, int count)
        {
            var theta = 2 * Mathf.PI / count;
            for (var i = 0; i < count; i++)
                SpawnEnemy(
                    new Vector2(r * Mathf.Cos(i * theta), r * Mathf.Sin(i * theta)));
        }

        public void SpawnCircles(float spawnRadius, float enemyRadius, int count)
        {
            var layer = 0;
            while (count > 0)
            {
                var maxSubCount = (int) (Mathf.PI *
                                         (spawnRadius + layer * enemyRadius * 2) /
                                         enemyRadius);
                var subCount = count > maxSubCount ? maxSubCount : count;
                SpawnCircle(spawnRadius + layer * enemyRadius * 2, subCount);
                layer++;
                count -= subCount;
            }
        }

        #endregion

        #region Debug

        [ContextMenu("Count Enemies")]
        private void DebugCountEnemies()
        {
            Debug.Log(transform.childCount);
        }

        [ContextMenu("Clear Enemies")]
        private void DebugClearEnemies()
        {
            if (Application.isPlaying)
                for (var i = transform.childCount - 1; i >= 0; i--)
                    Destroy(transform.GetChild(i).gameObject);
            else
                for (var i = transform.childCount - 1; i >= 0; i--)
                    DestroyImmediate(transform.GetChild(i).gameObject);
        }

        [ContextMenu("Spawn Enemy")]
        private void DebugSpawnEnemy()
        {
            SpawnEnemy(Vector2.zero);
        }

        [ContextMenu("Spawn Wave")]
        private void DebugSpawnWave()
        {
            SpawnCircles(
                _camera.orthographicSize * _camera.aspect + 1.75f,
                _enemyPrefab.GetComponent<CircleCollider2D>().radius,
                _debugSpawnWaveCount);
        }

        [ContextMenu("Respawn Wave")]
        private void DebugRespawnWave()
        {
            DebugClearEnemies();
            DebugSpawnWave();
        }

        #endregion
    }
}