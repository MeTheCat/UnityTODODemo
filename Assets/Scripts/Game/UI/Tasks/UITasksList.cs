using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UITasksList : MonoBehaviour
    {
        [SerializeField] private GameObject loadingIndicator;
        [SerializeField] private ScrollRect scrollView;

        public void SetLoading(bool isLoading)
        {
            scrollView.enabled = !isLoading;
            loadingIndicator.SetActive(isLoading);
        }
    }
}