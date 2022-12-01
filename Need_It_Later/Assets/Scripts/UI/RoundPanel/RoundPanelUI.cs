using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoundPanelUI : MonoBehaviour
{
    public Animator TargetAnimator;
    public TMP_Text TargetText;
    public CombatRoundManager TargetRoundManager;

    public void Update()
    {
        TargetText.text = "Round " +
                          (TargetRoundManager.ElapsedRounds + 1).ToString() +
                          " starts in " +
                          MakeTimeString();
    }

    public string MakeTimeString()
    {
        var remainingTime =
            TargetRoundManager.EndRoundDelay - TargetRoundManager.ElapsedTime;
        return remainingTime.ToString("0.0");
    }

    public void StartRoundPanelUI()
    {
        gameObject.SetActive(true);
        this.enabled = true;
        // TargetAnimator.SetTrigger("OnStartRoundPanelUI");
    }
    
    public void EndRoundPanelUI()
    {
        gameObject.SetActive(false);
        this.enabled = false;
        // TargetAnimator.SetTrigger("OnEndRoundPanelUI");
    }
    
}
