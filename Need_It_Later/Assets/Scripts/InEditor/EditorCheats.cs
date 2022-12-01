using Item;
using Singletons;
using UnityEngine;
using Random = UnityEngine.Random;

namespace InEditor
{
    public class EditorCheats : MonoBehaviour
    {
        [SerializeField] private ItemScriptableObject[] items;
        [SerializeField] private GameObject controls;
        private GameManager _gm;

        private void Start()
        {
            _gm = FindObjectOfType<GameManager>();
        }

        public void RandomItem()
        {
            var randomItem = items[Random.Range(0, items.Length)];
            _gm.GetMainInventoryItems().Add(randomItem);
            _gm.SetMainInventoryItems(_gm.GetMainInventoryItems());
            Debug.Log($"Added {randomItem.name} to inventory");
            //_gm.onGameStateChange.Invoke(GameManager.GameState.MainMenu);
        }

        public void Save()
        {
            _gm.SaveGame();
        }

        public void ToggleVisibility()
        {
            controls.SetActive(!controls.activeSelf);
        }
    }
}
