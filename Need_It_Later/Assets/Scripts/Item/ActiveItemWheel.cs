using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Item
{
    public class ActiveItemWheel : MonoBehaviour
    {
        public List<ActiveItemSlot> ItemSlots = new();
        public List<ItemScriptableObject> WheelEquippedItems = new();

        private void Start()
        {
            if (ItemSlots.Any())
            {
                ItemSlots.Sort(((lhs, rhs) => lhs.SlotID - rhs.SlotID ) );
            }
        }

        private void Update()
        {
            if (ItemSlots.Any(element => element.EquippedItem == null))
            {
                ItemSlots.ForEach(element => element.TryEquipAnyItem());
            }
        }
    }
}