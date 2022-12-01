using System.Collections.Generic;
using System.Linq;
using Item;
using UnityEngine;
using UnityEngine.UI;

public class ActiveItemWheelUI : MonoBehaviour
{
    public ActiveItemWheel TargetWheel;
    // public List<GameObject> Slices = new();
    public List<SpriteRenderer> SliceImages = new();

    // Update is called once per frame
    private void Update()
    {
        if (SliceImages.Count != TargetWheel.WheelEquippedItems.Count)
        {
            SliceImages = GetComponentsInChildren<SpriteRenderer>(true).ToList();
        }

        if (SliceImages.Count == TargetWheel.WheelEquippedItems.Count)
            SliceImages.ForEach(
                image => image.sprite = TargetWheel
                    .WheelEquippedItems[SliceImages.IndexOf(image)]
                    .Icon);
    }
}