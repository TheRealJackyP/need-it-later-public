using System.Collections;
using Health;
using Player;
using Singletons;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    public class UICanvas : MonoBehaviour
    {
        //private HotBarSlot[] _hotBarSlots;
        //private List<List<Vector3>> _hotBarSlotRects = new List<List<Vector3>>();
        //private HotBarSlot _currentHotBarSlot;

        [SerializeField] public Image loadingCircle;
        
        [SerializeField] private GameObject gameManagerPrefab;
        private GameManager _gameManager;
        
        [HideInInspector] public SelectionWheelMesh _selectionWheelMesh;
        
        private GameObject _player;
        
        [SerializeField] private GameObject deathScreen;
        
        private bool _fillingCircle = false;
    
        void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
            //When testing in editor, we can still use the GameManager without having to start from the main menu
            if (_gameManager == null)
            {
                _gameManager = Instantiate(gameManagerPrefab).GetComponent<GameManager>();
            }
            
            _gameManager.CurrentGameState = GameManager.GameState.Playing;
            //Rect transforms needed for UI aren't quite ready at start, so we delay the rest of the start logic
            Invoke(nameof(DelayedStart), .12f);
        }

        public void OnItemSlot1(InputValue value)
        {
            ItemSlotUniversal(value.isPressed);
        }
        
        public void OnItemSlot2(InputValue value)
        {
            ItemSlotUniversal(value.isPressed);
        }
        
        public void OnItemSlot3(InputValue value)
        {
            ItemSlotUniversal(value.isPressed);
        }
        
        public void OnItemSlot4(InputValue value)
        {
            ItemSlotUniversal(value.isPressed);
        }
        
        public void OnItemSlot1(bool value)
        {
            ItemSlotUniversal(value);
        }
        
        public void OnItemSlot2(bool value)
        {
            ItemSlotUniversal(value);
        }
        
        public void OnItemSlot3(bool value)
        {
            ItemSlotUniversal(value);
        }
        
        public void OnItemSlot4(bool value)
        {
            ItemSlotUniversal(value);
        }

        private void ItemSlotUniversal(bool pressed)
        {
            if (pressed)
            {
                _fillingCircle = true;
                StartCoroutine(FillCircle());
            }
            else
            {
                _fillingCircle = false;
                StopCoroutine(FillCircle());
                loadingCircle.fillAmount = 0;
                _selectionWheelMesh.gameObject.SetActive(false); //.enabled = false;
            }
        }
        
        private IEnumerator FillCircle()
        {
            while (loadingCircle.fillAmount < 1 && _fillingCircle)
            {
                loadingCircle.transform.position = Mouse.current.position.ReadValue();
                loadingCircle.fillAmount += Time.deltaTime;
                yield return null;
            }

            if (_fillingCircle)
            {
                loadingCircle.fillAmount = 0;
                _selectionWheelMesh.gameObject.SetActive(true); //.enabled = true;
            }

        }

        private void DelayedStart()
        {
            _selectionWheelMesh = FindObjectOfType<SelectionWheelMesh>();

            _player = GameObject.Find("Player");
            if (!_player)
            {
                var p = FindObjectOfType<PlayerAction>();
                if (!p)
                {
                    Debug.Log("No player found");
                    return;
                }
                _player = p.gameObject;
            }

            _player.GetComponent<EntityHealth>().OnEntityDie.AddListener(OnEntityDie);
        }

        private void OnEntityDie(EntityHealth health, GameObject obj)
        {
            deathScreen.SetActive(true);
        }
    }
}
