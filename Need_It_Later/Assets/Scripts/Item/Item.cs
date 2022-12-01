using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Item
{
    public class Item : MonoBehaviour
    {
        [Foldout("Events", foldEverything = true, styled = true, readOnly = false)] [SerializeField]
        private UnityEvent<Item, GameObject> OnPickedUp = new();

        [Foldout("Object References", foldEverything = true, styled = true, readOnly = false)] [SerializeField]
        private SpriteRenderer _spriteRenderer;

        [SerializeField] private Collider2D _collider;

        public ItemScriptableObject _itemScriptableObject;

        public PlayerInventoryHandler InventoryHandler;

        public static List<Item> ActiveItems = new();

        public Coroutine GatherCoroutine = null;

        private void OnEnable()
        {
            ActiveItems.Add(this);
        }

        public void Init(ItemScriptableObject itemScriptableObject)
        {
            _itemScriptableObject = itemScriptableObject;
            _spriteRenderer.sprite = _itemScriptableObject.Icon;
        }

        public ItemScriptableObject PickUp()
        {
            _collider.enabled = false;
            // OnPickedUp.Invoke();
            return _itemScriptableObject;
        }

        public void DestroySelf()
        {
            ActiveItems.Remove(this);
            if(GatherCoroutine != null)
                StopCoroutine(GatherCoroutine);
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent<PlayerInput>(out var playerInput))
            {
                if (InventoryHandler == null)
                {
                    InventoryHandler =
                        playerInput.GetComponent<PlayerInventoryHandler>();
                }
                InventoryHandler.HandleAddItem(_itemScriptableObject);
                OnPickedUp.Invoke(this, playerInput.gameObject);
                DestroySelf();
            }
        }

        public void StartGather(PlayerGather targetAbility)
        {
            if (GatherCoroutine == null)
            {
                GatherCoroutine = StartCoroutine(PerformGather(targetAbility));
            }
        }

        public IEnumerator PerformGather(PlayerGather targetAbility)
        {
            int elapsedTicks = 0;
            var displacement = transform.position - targetAbility.transform.position;
            if (InventoryHandler == null)
                InventoryHandler = targetAbility.GetComponent<PlayerInventoryHandler>();
            while(displacement.magnitude > 0.01f)
            {
                displacement = transform.position - targetAbility.transform.position;
                var delta = targetAbility.GatherDefaultLinearSpeed +
                            targetAbility.GatherSpeedMultiplier * elapsedTicks;
                transform.position -= displacement.normalized * (Mathf.Clamp(delta* Time.fixedDeltaTime, 0, displacement.magnitude) );
                elapsedTicks += 1;
                yield return null;
            }

            GatherCoroutine = null;
        }
    }
}
