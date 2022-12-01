using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Item;
using JetBrains.Annotations;
using Player;
using UnityEngine;
using UnityEngine.Events;

public class ActiveItemSlot : MonoBehaviour
{
    public ItemScriptableObject EquippedItem;
    public PlayerStats TargetStats;
    public ActiveItemWheel TargetWheel;
    public PlayerInventoryHandler InventoryHandler;

    public int SlotID;

    private void Start()
    {
        if (EquippedItem == null && TargetWheel.WheelEquippedItems.Count < SlotID)
        {
            HandlePlayerInputSwitchItem(null, null, SlotID, SlotID);
        }

        if (EquippedItem != null && InventoryHandler.CheckItemQuantity(EquippedItem) == 0) EquippedItem = null;
    }

    private void Update()
    {
        if (EquippedItem != null && InventoryHandler.CheckItemQuantity(EquippedItem) == 0) EquippedItem = null;
    }

    public void HandlePlayerInputItemUse(PlayerUseItem itemAction, GameObject playerObject, int slotID)
    {
        if (slotID - 1 == SlotID && EquippedItem != null)
        {
            TargetStats.DoApplyBuff(EquippedItem);
        }
    }

    public void TryEquipAnyItem()
    {
        //Any Item that is not equipped in an item slot and has a non-zero quantity.
        if (TargetWheel.WheelEquippedItems.Any(
                targetItem =>
                    TargetWheel.ItemSlots.All(
                        itemSlot => itemSlot.EquippedItem != targetItem) &&
                    InventoryHandler.CheckItemQuantity(targetItem) > 0))
            EquippedItem = TargetWheel.WheelEquippedItems.First(
                targetItem =>
                    TargetWheel.ItemSlots.All(
                        itemSlot => itemSlot.EquippedItem != targetItem) &&
                    InventoryHandler.CheckItemQuantity(targetItem) > 0);
    }
    
    

    public void HandlePlayerInputSwitchItem(
        PlayerUseItem itemAction,
        GameObject playerObject,
        int slotID,
        int sliceID)
    {
        if (slotID - 1 != SlotID) return;
        if (sliceID >= TargetWheel.WheelEquippedItems.Count) return;
        var newItem = TargetWheel.WheelEquippedItems[sliceID];
        if (TargetWheel.ItemSlots.Any(
                element => (element.SlotID != SlotID) &&
                           (element.EquippedItem == newItem)))
        {
            TargetWheel.ItemSlots.First(
                    element => (element.SlotID != SlotID) &&
                               (element.EquippedItem == newItem))
                .EquippedItem = EquippedItem;
            EquippedItem = newItem;
        }
        else
        {
            EquippedItem = newItem;
        }
    }
}
