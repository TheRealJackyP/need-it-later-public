using System;
using System.Collections.Generic;
using Item;
using Singletons;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class UIInventory : MonoBehaviour
    {
        public UnityEvent onInventoryUpdated;
        private GameManager _gameManager;
        
        [SerializeField] private GameObject inventoryPanel;
        [SerializeField] private GameObject inventorySlot;
        [SerializeField] private GameObject gameManagerPrefab;
        
        [SerializeField] private ItemStatsDisplay itemStatsDisplay;
        private List<GameObject> _slots = new List<GameObject>();

        [SerializeField] private GameObject wheelItems;
        
        private Image[][] _wheelItemsImages;
        [SerializeField] private Button[] wheelButtons; 

        [SerializeField] private Image[] wheelOne;
        [SerializeField] private Image[] wheelTwo;
        [SerializeField] private Image[] wheelThree;
        [SerializeField] private Image[] wheelFour;
        
 
        
        private void Start()
        {
            _wheelItemsImages = new Image[4][];
            _wheelItemsImages[0] = wheelOne;
            _wheelItemsImages[1] = wheelTwo;
            _wheelItemsImages[2] = wheelThree;
            _wheelItemsImages[3] = wheelFour;
            
            _gameManager = FindObjectOfType<GameManager>();
            //When testing in editor, we can still use the GameManager without having to start from the main menu
            if (_gameManager == null)
            {
                _gameManager = Instantiate(gameManagerPrefab).GetComponent<GameManager>();
            }
            
            _gameManager.CurrentGameState = GameManager.GameState.Playing;
            Invoke(nameof(DelayedStart), .5f);
            Invoke(nameof(UpdateInventory), .1f);
            itemStatsDisplay = FindObjectOfType<ItemStatsDisplay>();
            //item.onItemClicked.AddListener(OnItemClicked);
        }

        private void DelayedStart()
        {
            _gameManager.onMainInventoryChanged.AddListener(UpdateInventory);
        }
        
        private void OnItemClicked(int id)
        {
            Debug.Log("Clicked on " + id);
        }
        
        private void OnHover(int id, bool isHovered)
        {
            itemStatsDisplay.SetStatsText(_gameManager.GetMainInventoryItems()[id]);
            //Debug.Log("Hovering on " + id + " " + isHovered);
        }
        
        public void UpdateInventory()
        {
            //Debug.Log("Updating inventory");
            foreach (var slot in _slots)
            {
                Destroy(slot);
            }

            for (var i = 0; i < _gameManager.GetMainInventoryItems().Count; i++)
            {
                var invItem = _gameManager.GetMainInventoryItems()[i];
                var item = Instantiate(inventorySlot, inventoryPanel.transform);
                var itemButton = item.GetComponentInChildren<Button>(); //onItemClicked.AddListener(OnItemClicked);
                var i1 = i;
                itemButton.onClick.AddListener(() => OnItemClicked(i1));
                var hoverEvent = item.gameObject.GetComponent<InventorySlot>().onHover;

                void Call(bool isHovered) => OnHover(i1, isHovered);

                hoverEvent.AddListener(Call);
                item.name = invItem.Name;
                itemButton.GetComponent<Image>().sprite = invItem.Icon;
                _slots.Add(item);
            }
            
            for (var i = 0; i < _gameManager.InventoryWheels.Length; i++)
            {
                var wheel = _gameManager.InventoryWheels[i];
                for (var j = 0; j < 10; j++) //wheel.Length
                {
                    ItemScriptableObject item = null;
                    if (wheel[j] != null)
                    {
                        item = wheel[j];
                    }
                    
                    _wheelItemsImages[i][j].sprite = item != null ? item.Icon : null;
                }
            }
        }

        
        //This will work once we add names to the items
        public void InventorySearch(string term)
        {
            //Debug.Log("Searching for " + term);
            foreach (var slot in _slots)
            {
                slot.SetActive(slot.name.ToLower().Contains(term.ToLower()));
            }
        }
        
        public void ToggleWindow(int windowID)
        {
            for (var i = 0; i < 10; i++)
            {
                wheelButtons[i].image.sprite = _wheelItemsImages[windowID][i].sprite;
            }
            wheelItems.SetActive(!wheelItems.activeSelf);
        }
        
    }
}
