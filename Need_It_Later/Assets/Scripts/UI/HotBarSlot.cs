using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Singletons;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    public class HotBarSlot: MonoBehaviour
    {
        private bool _mouseDown;
        private Vector2 _initialMousePosition;
        
        [SerializeField] private GameObject itemSelection;
        private SubItemBox _currentSubItemBox;
        internal List<SubItemBox> SubItemBoxes = new List<SubItemBox>();

        private bool _quickUse;

        private Image _loadingCircle;
        
        private List<Coroutine> _coroutines = new List<Coroutine>();

        private void Start()
        {
            Invoke(nameof(DelayedStart), .75f);
            _loadingCircle = FindObjectOfType<UICanvas>().loadingCircle;
        }

        private void DelayedStart()
        {
            itemSelection.SetActive(false);
            FindObjectOfType<GameManager>().loadingScreen.SetActive(false);
        }
        
        public void OnClick(InputValue value)
        {
            
            
            _mouseDown = value.isPressed;
            _initialMousePosition = Mouse.current.position.ReadValue();
            

            if (_mouseDown)
            {
                _loadingCircle.fillAmount = 0;
                _coroutines.Add(StartCoroutine(DragLogic()));
                StartCoroutine(FillCircle());
                _quickUse = true;

                _loadingCircle.gameObject.SetActive(true);
                _loadingCircle.gameObject.transform.position = _initialMousePosition;
            }
            else
            {
                StopCoroutine(FillCircle());
                _loadingCircle.gameObject.SetActive(false);
                _loadingCircle.fillAmount = 0;

                if (_quickUse)
                {
                    QuickUse();
                    StopCoroutine(_coroutines[0]);
                    _coroutines.RemoveAt(0);
                    itemSelection.SetActive(false);
                }
            }
        }
        
        private IEnumerator FillCircle()
        {
            while (_loadingCircle.fillAmount < 1)
            {
                _loadingCircle.fillAmount += Time.deltaTime;
                yield return null;
            }
        }


        private IEnumerator DragLogic()
        {
            
            yield return new WaitForSecondsRealtime(1f);
            
            _loadingCircle.gameObject.SetActive(false);
            _loadingCircle.fillAmount = 0;
            
            _quickUse = false;
            
            var mousePos = Mouse.current.position.ReadValue();
            foreach (var box in SubItemBoxes)
            {
                if (mousePos.x >= box.XBounds.x && mousePos.x <= box.XBounds.y)
                {
                    _currentSubItemBox = box;
                    _currentSubItemBox.highlight.enabled = true;
                    break;
                }
                //Debug.Log(box.XBounds);
            }
            itemSelection.SetActive(true);
            while (_mouseDown)
            {
                mousePos = Mouse.current.position.ReadValue();
                
                //This null check can probably go once the code is more tested
                if (_currentSubItemBox == null)
                {
                    Debug.Log("Current sub item box is null");
                }
                if (mousePos.x <= _currentSubItemBox.XBounds.x || mousePos.x >= _currentSubItemBox.XBounds.y)
                {
                    _currentSubItemBox.highlight.enabled = false;
                    foreach (var box in SubItemBoxes.Where(box => mousePos.x >= box.XBounds.x && mousePos.x <= box.XBounds.y))
                    {
                        _currentSubItemBox = box;
                        _currentSubItemBox.highlight.enabled = true;
                        break;
                    }
                }
                
                //yield break;
                //Debug.Log(Mouse.current.position.ReadValue());
                yield return null;
            }

            if (_currentSubItemBox)
            {
                _currentSubItemBox.highlight.enabled = false;
                Debug.Log("Extended item used");
            }

            itemSelection.SetActive(false);
            _coroutines.RemoveAt(0);
        }
        
        
        private void QuickUse()
        {
            Debug.Log("Quick use item");
        }
    }
}
