using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerXPUI : MonoBehaviour
{
    public PlayerXP TargetXP;
    public Slider TargetSlider;

    // Update is called once per frame
    void Update()
    {
        TargetSlider.value = Mathf.Clamp01(
            TargetXP.CurrentXP / TargetXP.BaseXP);
    }
}
