using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI.Common
{
    public class UIButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] Button button;

        public Action OnClick;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!button.interactable) return;
            OnClick?.Invoke();
        }

        internal void SetInteractable(bool interactable)
        {
            this.button.interactable = interactable;
        }
    }
}