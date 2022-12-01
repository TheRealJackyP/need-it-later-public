using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActiveItemUI : MonoBehaviour
{
    public Image TargetImage;
    public ActiveItemSlot TargetSlot;
    public TMP_Text TargetText;
    public GameObject TargetTextPanel;

    // Update is called once per frame
    void Update()
    {
        if (TargetSlot.EquippedItem == null)
        {
            TargetImage.enabled = false;
            TargetTextPanel.SetActive(false);
        }

        else
        {
            TargetImage.enabled = true;
            TargetTextPanel.SetActive(true);
            TargetImage.sprite = TargetSlot.EquippedItem.Icon;
            if (TargetSlot.InventoryHandler.PlayerItemQuantities.TryGetValue(
                    TargetSlot.EquippedItem,
                    out var quantity))
                TargetText.text = quantity < 100 ? quantity.ToString() : "+99";
        }
    }
}
