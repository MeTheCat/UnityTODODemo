using System;
using Game.UI.Common;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class UITaskItem : MonoBehaviour
    {
        public class UITaskItemData
        {
            public string Content;
            public int Id;
            public Action<UITaskItem> OnEditClick;
            public Action<UITaskItem, int, string> OnSaveClick;
            public Action OnDeleteClick;
            public Action OnDoneClick;
        }

        //[SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI contentText;
        [SerializeField] private TMP_InputField contentTextField;
        [SerializeField] private UIButton buttonDelete, buttonEdit, buttonSave;

        private UITaskItemData data;

        public void SetData(UITaskItemData data)
        {
            this.data = data;
            contentText.text = data.Content;
            contentTextField.text = data.Content;
            SetTextEditable(false);
            //Recalculate content size?
        }

        private void Awake()
        {
            buttonDelete.OnClick -= OnDeleteClick;
            buttonDelete.OnClick += OnDeleteClick;

            buttonEdit.OnClick -= OnEditClick;
            buttonEdit.OnClick += OnEditClick;

            buttonSave.OnClick -= OnSaveClick;
            buttonSave.OnClick += OnSaveClick;
        }

        public void SetTextEditable(bool isEditable)
        {
            contentTextField.enabled = isEditable;
            buttonEdit.gameObject.SetActive(!isEditable);
            buttonSave.gameObject.SetActive(isEditable);
        }

        public string GetText()
        {
            return contentText.text;
        }

        private void OnEditClick()
        {
            this.data?.OnEditClick?.Invoke(this);
        }

        private void OnDeleteClick()
        {
            this.data?.OnDeleteClick?.Invoke();
        }

        private void OnSaveClick()
        {
            this.data?.OnSaveClick?.Invoke(this, data.Id, contentTextField.text);
        }
    }
}