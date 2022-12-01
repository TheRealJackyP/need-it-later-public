using System;
using System.Linq;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Weapon;
using Random = UnityEngine.Random;

namespace Player
{
    public class PlayerUseItem : InputPlayerAction
    {
        [Foldout(
            "Player Use Parameters",
            foldEverything = true,
            styled = true,
            readOnly = false)]
        public int ItemSlot;
        
        [Foldout(
            "Player Use References",
            foldEverything = true,
            styled = true,
            readOnly = false)]
        public UICanvas TargetCanvas;

        public PlayerInventoryHandler InventoryHandler;

        public SelectionWheelController TargetWheel;
        [Foldout(
            "Player Use Events",
            foldEverything = true,
            styled = true,
            readOnly = false)]
        public UnityEvent<PlayerUseItem, GameObject, int> OnPlayerInputItemUse = new();
        public UnityEvent<PlayerUseItem, GameObject, int, int> OnPlayerInputSwitchItem = new();
        private bool finishInput;

        public override bool CompleteExecuteWait()
        {
            return finishInput;
        }

        public override void HandleInputStarted(InputAction.CallbackContext context)
        {
            base.HandleInputStarted(context);
            switch (ItemSlot)
            {
                case 1:
                    TargetCanvas.OnItemSlot1(true);
                    break;
                case 2:
                    TargetCanvas.OnItemSlot2(true);
                    break;
                case 3:
                    TargetCanvas.OnItemSlot3(true);
                    break;
                case 4:
                    TargetCanvas.OnItemSlot4(true);
                    break;
                default:
                    throw new ArgumentException(
                        "Invalid Item slot: " + ItemSlot + " on PlayerUseItem.");
            }

            finishInput = false;
            TryActivatePlayerAction();
        }

        public override void HandleInputStopped(InputAction.CallbackContext context)
        {
            finishInput = true;
            var wheelActive = TargetCanvas._selectionWheelMesh.gameObject
                .activeSelf;
            switch (ItemSlot)
            {
                case 1:
                    if (TargetCanvas.loadingCircle.fillAmount < 1 && !wheelActive )
                    {
                        var equippedItem = InventoryHandler.ItemSlots[ItemSlot - 1]
                            .EquippedItem;
                        if(equippedItem != null && InventoryHandler.CheckItemQuantity(equippedItem) > 0)
                            OnPlayerInputItemUse.Invoke(this, gameObject, 1);
                    }
                    else if (wheelActive)
                    {
                        var lastSelected = TargetWheel
                            .GetComponent<SelectionWheelMesh>()
                            ._lastSelected;
                        OnPlayerInputSwitchItem.Invoke(this, gameObject, 1, lastSelected);
                    }
                    TargetCanvas.OnItemSlot1(false);
                    break;
                case 2:
                    if (TargetCanvas.loadingCircle.fillAmount < 1 && !wheelActive)
                    {
                        var equippedItem = InventoryHandler.ItemSlots[ItemSlot - 1]
                            .EquippedItem;
                        if(equippedItem != null && InventoryHandler.CheckItemQuantity(equippedItem) > 0)
                            OnPlayerInputItemUse.Invoke(this, gameObject, 2);
                    }
                    else if (wheelActive)
                    {
                        var lastSelected = TargetWheel
                            .GetComponent<SelectionWheelMesh>()
                            ._lastSelected;
                        OnPlayerInputSwitchItem.Invoke(this, gameObject, 2, lastSelected);
                    }
                    TargetCanvas.OnItemSlot2(false);
                    break;
                case 3:
                    if (TargetCanvas.loadingCircle.fillAmount < 1 && !wheelActive)
                    {
                        var equippedItem = InventoryHandler.ItemSlots[ItemSlot - 1]
                            .EquippedItem;
                        if(equippedItem != null && InventoryHandler.CheckItemQuantity(equippedItem) > 0)
                            OnPlayerInputItemUse.Invoke(this, gameObject, 3);
                    }
                    else if (wheelActive)
                    {
                        var lastSelected = TargetWheel
                            .GetComponent<SelectionWheelMesh>()
                            ._lastSelected;
                        OnPlayerInputSwitchItem.Invoke(this, gameObject, 3, lastSelected);
                    }
                    TargetCanvas.OnItemSlot3(false);
                    break;
                case 4:
                    if (TargetCanvas.loadingCircle.fillAmount < 1 && !wheelActive)
                    {
                        var equippedItem = InventoryHandler.ItemSlots[ItemSlot - 1]
                            .EquippedItem;
                        if(equippedItem != null && InventoryHandler.CheckItemQuantity(equippedItem) > 0)
                            OnPlayerInputItemUse.Invoke(this, gameObject, 4);
                    }
                    else if (wheelActive)
                    {
                        var lastSelected = TargetWheel
                            .GetComponent<SelectionWheelMesh>()
                            ._lastSelected;
                        OnPlayerInputSwitchItem.Invoke(this, gameObject, 4, lastSelected);
                    }
                    TargetCanvas.OnItemSlot4(false);
                    break;
                default:
                    throw new ArgumentException(
                        "Invalid Item slot: " + ItemSlot + " on PlayerUseItem.");
            }
        }
    }
}