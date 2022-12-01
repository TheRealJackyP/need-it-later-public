using System.Collections;
using Singletons;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject gameManagerPrefab;
        private GameManager _gameManager;
        
        
        void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
            if (_gameManager == null)
            {
                _gameManager = Instantiate(gameManagerPrefab).GetComponent<GameManager>();
            }
            _gameManager.CurrentGameState = GameManager.GameState.MainMenu;
        }

        public void OnPlay()
        {
            _gameManager.loadingScreen.SetActive(true);
            _gameManager.PlayUISound();
            StartCoroutine(SceneSwitch(3));
        }

        IEnumerator SceneSwitch(int sceneIndex)
        {
            yield return new WaitForSeconds(.2f);
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
