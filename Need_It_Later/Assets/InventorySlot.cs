using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler , IPointerExitHandler
{
    public UnityEvent<bool> onHover = new UnityEvent<bool>();
    public void OnPointerEnter(PointerEventData eventData)
    {
        onHover.Invoke(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onHover.Invoke(false);
    }
    
}
