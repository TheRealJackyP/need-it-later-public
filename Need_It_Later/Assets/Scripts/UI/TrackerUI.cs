using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;

public class TrackerUI : MonoBehaviour
{
    public CombatRoundManager TargetRoundManager;

    public PlayerXP TargetPlayerXP;
    public DeathScreen TargetDeathScreen;

    public TMP_Text RoundText;

    public TMP_Text LevelText;

    public TMP_Text ScoreText;
    // Update is called once per frame
    void Update()
    {
        RoundText.text = TargetRoundManager.ElapsedRounds.ToString();
        LevelText.text = TargetPlayerXP.CurrentLevel.ToString();
        ScoreText.text = TargetDeathScreen.MakeTotalText().Replace("Final Score: ", "");
    }
}
