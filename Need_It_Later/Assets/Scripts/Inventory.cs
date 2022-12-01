using System.Collections.Generic;
using Item;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    internal List<ItemScriptableObject> Items { get; set; } = new List<ItemScriptableObject>();

    internal ItemScriptableObject[][] SelectionWheels = new ItemScriptableObject[4][];
    
}
