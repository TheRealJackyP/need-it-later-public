using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Enemy;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class DeathScreen : MonoBehaviour
    {
        private GameManager _gm;

        public List<string> LossStrings = new();
        public TMP_Text TitleText;
        public TMP_Text SubtitleText;
        public TMP_Text EnemiesText;
        public TMP_Text ItemsText;
        public TMP_Text RoundsText;
        public TMP_Text TotalText;
        public PlayerInventoryHandler TargetInventory;
        public EnemyManagerNew TargetEnemyManager;
        public CombatRoundManager TargetRoundManager;
        
        private void Start()
        {
            _gm = FindObjectOfType<GameManager>();
        }
        
        public void Restart()
        {
            StartCoroutine(ChangeScene(SceneManager.GetActiveScene().buildIndex));
        }
        
        public void Quit()
        {
            StartCoroutine(ChangeScene(0));
            //Application.Quit();
        }
        
        private IEnumerator ChangeScene(int sceneIndex)
        {
            _gm.loadingScreen.SetActive(true);
            yield return new WaitForSecondsRealtime(.1f);
            SceneManager.LoadScene(sceneIndex);
        }

        private void OnEnable()
        {
            _gm = FindObjectOfType<GameManager>();
            _gm.onGameStateChange.Invoke(GameManager.GameState.GameOver);
            TitleText.text = LossStrings[Random.Range(0, LossStrings.Count)];
            SubtitleText.text = MakeSubtitleText();
            EnemiesText.text = TargetEnemyManager.EnemiesDead.ToString();
            ItemsText.text = MakeItemsText();
            RoundsText.text = TargetRoundManager.ElapsedRounds.ToString();
            TotalText.text = MakeTotalText();
            //Time.timeScale = 0;
        }

        public string MakeSubtitleText()
        {
            if (!TargetInventory.PlayerItemQuantities.Any() ||
                !TargetInventory.PlayerItemQuantities.Any(element => element.Value > 0))
            {
                return "Guess I should have saved some items for later...";
            }
            else
            {
                var maxValue = TargetInventory.PlayerItemQuantities.Values.Max();
                return "But I might need those " +
                       maxValue +
                       " " +
                       TargetInventory.PlayerItemQuantities
                           .First(pair => pair.Value == maxValue)
                           .Key.Name + " later...";
            }
            
        }

        public string MakeItemsText()
        {
            if (!TargetInventory.PlayerItemQuantities.Any() ||
                !TargetInventory.PlayerItemQuantities.Any(element => element.Value > 0))
            {
                return "0";
            }
            else
            {
                return TargetInventory.PlayerItemQuantities.Values.Aggregate(0,
                    ((total, next) => total + next)).ToString();
            }
        }

        public string MakeTotalText()
        {
            var itemCount = 0;
            if (!TargetInventory.PlayerItemQuantities.Any() ||
                !TargetInventory.PlayerItemQuantities.Any(element => element.Value > 0))
            {
                itemCount = 0;
            }
            else
            {
                itemCount = TargetInventory.PlayerItemQuantities.Values.Aggregate(0,
                    ((total, next) => total + next));
            }

            var totalScore = (itemCount * TargetRoundManager.ElapsedRounds * .1f) + TargetEnemyManager.EnemiesDead;
            return "Final Score: " + totalScore.ToString(CultureInfo.InvariantCulture);
        }
    }
}
