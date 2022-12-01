using System.Collections.Generic;
using System.Linq;
using Item;
using Player;
using UnityEngine;

public class PlayerInventoryHandler : MonoBehaviour
{
    public List<ActiveItemSlot> ItemSlots = new();

    public SerializableDictionary<ItemScriptableObject, int> PlayerItemQuantities =
        new();

    public void HandlePlayerUseItem(
        PlayerUseItem playerItemAbility,
        GameObject playerObject,
        int slotID)
    {
        HandleUseItem(ItemSlots[slotID - 1].EquippedItem);
    }

    public void HandleUseItem(ItemScriptableObject usedObject, int amount = 1)
    {
        if (PlayerItemQuantities.TryGetValue(usedObject, out var quantity) &&
            amount <= quantity)
            PlayerItemQuantities[usedObject] -= amount;
    }

    public void HandleAddItem(ItemScriptableObject addedObject, int amount = 1)
    {
        if (PlayerItemQuantities.Keys.Contains(addedObject))
            PlayerItemQuantities[addedObject] += amount;

        else
            PlayerItemQuantities.Add(addedObject, amount);

        if (ItemSlots.Any(element => element.EquippedItem == null))
            ItemSlots.Where(element => element.EquippedItem == null)
                .ToList()
                .ForEach(element => element.TryEquipAnyItem());
    }

    public int CheckItemQuantity(ItemScriptableObject targetObject)
    {
        if (PlayerItemQuantities.TryGetValue(targetObject, out var quantity))
            return quantity;

        return 0;
    }
}