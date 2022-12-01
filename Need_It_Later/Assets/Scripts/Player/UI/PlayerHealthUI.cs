using System;
using System.Collections;
using System.Collections.Generic;
using Health;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public EntityHealth TargetEntityHealth;
    public Slider TargetSlider;
    public TMP_Text TargetText;

    private void Update()
    {
        TargetText.text = TargetEntityHealth.CurrentHealth.ToString("0.#") +
                          " / " +
                          TargetEntityHealth.BaseHealth;
        TargetSlider.value = Mathf.Clamp01(
            TargetEntityHealth.CurrentHealth / TargetEntityHealth.BaseHealth);
    }
}
