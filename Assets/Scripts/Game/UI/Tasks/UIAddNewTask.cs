using System;
using Game.UI.Common;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class UIAddNewTask : MonoBehaviour
    {
        [SerializeField] private TMP_InputField textInputContent;
        [SerializeField] private UIButton buttonAdd;

        public Action<string> OnSubmitAdd;

        public void Awake()
        {
            buttonAdd.OnClick -= ButtonAddHandler;
            buttonAdd.OnClick += ButtonAddHandler;
        }

        public void SetLoading(bool isLoading)
        {
            textInputContent.interactable = !isLoading;
            buttonAdd.SetInteractable(!isLoading);
            //Show loading circle
        }

        private void ButtonAddHandler()
        {
            OnSubmitAdd?.Invoke(textInputContent.text);
        }
    }
}