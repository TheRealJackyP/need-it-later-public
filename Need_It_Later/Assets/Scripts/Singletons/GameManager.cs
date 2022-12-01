using System;
using System.Collections.Generic;
using Item;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Singletons
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                return;
            }
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }

        public enum GameState
        {
            MainMenu,
            Playing,
            Paused,
            GameOver
        }

        public List<ItemScriptableObject> GetMainInventoryItems() => _mainInventoryItems;

        public void SetMainInventoryItems(List<ItemScriptableObject> value)
        {
            Debug.Log("Main Inventory Changed");
            _mainInventoryItems = value;
            onMainInventoryChanged.Invoke();
        }
        
        internal ItemScriptableObject[][] InventoryWheels = new ItemScriptableObject[4][];

        public UnityEvent onMainInventoryChanged = new UnityEvent();
        public UnityEvent<GameState> onGameStateChange = new UnityEvent<GameState>();
        private GameState _currentGameState = GameState.MainMenu;
        internal GameState CurrentGameState
        {
            get => _currentGameState;
            set
            {
                _currentGameState = value;
                onGameStateChange.Invoke(_currentGameState);
            }
        }
        
        public Sprite testSprite;
        
        [SerializeField] internal GameObject loadingScreen;

        [SerializeField] private GameObject editorCheats;

        [Header("Audio")]
        [SerializeField] private AudioSource music;
        [SerializeField] private AudioSource sfx;
        
        [SerializeField] private AudioClip uiSound;
        [SerializeField] private AudioClip errorSound;
        private List<ItemScriptableObject> _mainInventoryItems;


        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            onGameStateChange.AddListener(OnGameStateChange);

            editorCheats.SetActive(Application.isEditor);

            SetMainInventoryItems(new List<ItemScriptableObject>()); //gameObject.AddComponent<Inventory>();
            var inv = PlayerPrefs.GetString("inventory");
            //Debug.Log(inv);
            var separated = inv.Split("//");
            foreach (var item in separated)
            {
                if (item == "") continue;
                var tempItem = ScriptableObject.CreateInstance<ItemScriptableObject>();
                JsonUtility.FromJsonOverwrite(item, tempItem); //<ItemScriptableObject>(item));
                if (tempItem.Icon != null)
                {
                    GetMainInventoryItems().Add(tempItem);
                }

                /*var itemScriptableObject = Resources.Load<ItemScriptableObject>("Items/" + item);
                MyInventoryItems.Add(itemScriptableObject);*/
            }
            
            var wheels = PlayerPrefs.GetString("wheels");
            var separatedWheels = wheels.Split("//");
            if (separatedWheels.Length > 1)
            {
                //Debug.Log("Loading Wheels");
                for (var i = 0; i < separatedWheels.Length; i++)
                {
                    var wheel = separatedWheels[i];
                    if (wheel == "") continue;
                    var separatedWheel = wheel.Split(";");
                    var wheelItems = new ItemScriptableObject[separatedWheel.Length];
                    for (var j = 0; j < separatedWheel.Length; j++)
                    {
                        var item = separatedWheel[j];
                        if (item == "") continue;
                        var tempItem = ScriptableObject.CreateInstance<ItemScriptableObject>();
                        JsonUtility.FromJsonOverwrite(item, tempItem); //<ItemScriptableObject>(item));
                        if (tempItem.Icon != null)
                        {
                            wheelItems[j] = tempItem;
                        }
                    }
                    InventoryWheels[i] = wheelItems;
                }
                SetMainInventoryItems(GetMainInventoryItems() ?? new List<ItemScriptableObject>());
            }
            else
            {
                //Debug.Log("No wheels found");
                InventoryWheels = new ItemScriptableObject[4][];
                //for (InventoryWheels)
                InventoryWheels[0] = new ItemScriptableObject[10];
                InventoryWheels[1] = new ItemScriptableObject[10];
                InventoryWheels[2] = new ItemScriptableObject[10];
                InventoryWheels[3] = new ItemScriptableObject[10];
                for (var i = 0; i < InventoryWheels.Length; i++)
                {
                    for (var j = 0; j < InventoryWheels[i].Length; j++)
                    {
                        InventoryWheels[i][j] = ScriptableObject.CreateInstance<ItemScriptableObject>();
                    }
                }
            }


        }

        private void OnGameStateChange(GameState state)
        {
            Debug.Log($"Game State Changed to {state}");

            SaveGame();
            //Debug.Log(PlayerPrefs.GetString("inventory"));
            
            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    //music.Play();
                    break;
                case GameState.Playing:
                    //music.Play();
                    break;
                case GameState.Paused:
                    //music.Pause();
                    break;
                case GameState.GameOver:
                    //music.Stop();
                    break;
                default:
                    Debug.LogError("Game state change logic missing");
                    break;
            }
        }
        
        public void PlayUISound(float volume = 1f)
        {
            sfx.PlayOneShot(uiSound, volume);
        }
        
        public void PlayErrorSound(float volume = 1f)
        {
            sfx.PlayOneShot(errorSound, volume);
        }
        
        public void PlayMusic(AudioClip clip, float volume = 1f)
        {
            music.clip = clip;
            music.volume = volume;
            music.Play();
        }
        
        public void PlaySfx(AudioClip clip, float volume = 1f)
        {
            sfx.PlayOneShot(clip, volume);
        }

        internal void SaveGame()
        {
            var toSave = "";
            for (int i = 0; i < GetMainInventoryItems().Count; i++)
            {
                toSave += JsonUtility.ToJson(GetMainInventoryItems()[i]) + "//";
                //print(toSave);
            }
            var wheelSave = "";
            for (var i = 0; i < InventoryWheels.Length; i++)
            {
                for (var j = 0; j < InventoryWheels[i].Length; j++)
                {
                    wheelSave += JsonUtility.ToJson(InventoryWheels[i][j]) + "//";
                }
            }
            
            PlayerPrefs.SetString("inventory", toSave);
            PlayerPrefs.SetString("inventoryWheels", wheelSave);
            PlayerPrefs.Save();
            Debug.Log("Game Saved");
        }
        

    }
}
