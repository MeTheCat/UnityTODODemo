using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITasksList : MonoBehaviour
{
    [SerializeField] private GameObject loadingIndicator;
    [SerializeField] private ScrollRect scrollView;

    public void SetData(List<UITaskItem.UITaskItemData> taskItemsData)
    {
        
    }

    public void SetLoading(bool isLoading)
    {
        scrollView.enabled = !isLoading;
        loadingIndicator.SetActive(isLoading);
    }
}