using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SubItemBox : MonoBehaviour
    {

        [SerializeField] internal Image highlight;
        [SerializeField] internal Image icon;
        [SerializeField] internal RectTransform rectTransform;
        internal HotBarSlot MySlot;
        internal Vector2 XBounds;


        private void Start()
        {
            MySlot = GetComponentInParent<HotBarSlot>();
            MySlot.SubItemBoxes.Add(this);
            Invoke(nameof(DelayedStart), .05f);
        }
        
        private void DelayedStart()
        {
            Vector3[] vec = new Vector3[4];
            rectTransform.GetWorldCorners(vec);
            XBounds = new Vector2(vec[0].x, vec[3].x);
        }

        private void OnEnable()
        {
            Vector3[] vec = new Vector3[4];
            rectTransform.GetWorldCorners(vec);
            XBounds = new Vector2(vec[0].x, vec[3].x);
        }

        private void OnDisable()
        {
            highlight.enabled = false;
        }
    }
}
