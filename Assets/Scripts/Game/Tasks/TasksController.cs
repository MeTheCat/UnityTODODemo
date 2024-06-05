using Data;
using Game.UI;
using GameServices;
using UnityEngine;

namespace Game.Tasks
{
    public class TasksController : MonoBehaviour
    {
        [SerializeField] private UITasksList uITasksList;
        [SerializeField] private UITaskItem uITaskItemPrefab;
        [SerializeField] private UIAddNewTask uIAddNewTaskComponent;
        [SerializeField] private Transform scrollContentParent;
        private UserDataService userDataService;

        public void Init()
        {
            userDataService = Services.Instance.Get<UserDataService>();

            GetTasksData();

            uIAddNewTaskComponent.OnSubmitAdd -= AddTaskClicked;
            uIAddNewTaskComponent.OnSubmitAdd += AddTaskClicked;
        }

        private async void GetTasksData()
        {
            this.uITasksList.SetLoading(true);
            this.uIAddNewTaskComponent.SetLoading(true);

            try
            {
                await userDataService.FetchInitialDataAsync();
            }
            catch
            {
                //TODO: Show error/retry button
                return;
            }

            var tasks = userDataService.GetUserData().TaskItemsById;

            foreach (var t in tasks)
            {
                InstantiateTaskItem(t.Value);
            }

            this.uITasksList.SetLoading(false);
            this.uIAddNewTaskComponent.SetLoading(false);
        }

        private void InstantiateTaskItem(TaskItem t)
        {
            var taskItem = Instantiate(uITaskItemPrefab, scrollContentParent);
            taskItem.SetData(new UITaskItem.UITaskItemData()
            {
                Id = t.id,
                Content = t.title,
                OnDoneClick = () => Debug.Log("Mark done"),
                OnDeleteClick = () => Debug.Log("Delete"),
                OnEditClick = (item) => item.SetTextEditable(true),
                OnSaveClick = SaveTaskClicked
            });
        }

        private async void AddTaskClicked(string text)
        {
            if (HandleEmptyString(text)) return;

            uIAddNewTaskComponent.SetLoading(true);

            try
            {
                var newItem = await userDataService.AddTaskAsync(text);
                InstantiateTaskItem(newItem);
            }
            catch
            {
                //Show error in UI
            }

            uIAddNewTaskComponent.SetLoading(false);
        }

        private async void SaveTaskClicked(UITaskItem item, int taskId, string text)
        {
            if (HandleEmptyString(text)) return;

            item.SetTextEditable(false);

            if (userDataService.GetUserData().TaskItemsById[taskId].title.Equals(text))
            {
                return;
            }

            try
            {
                await userDataService.EditTaskAsync(taskId, text);
            }
            catch
            {
                //Show error in UI
                //Either keep entered text, or revert to the original
            }
        }

        private bool HandleEmptyString(string text)
        {
            return false;
            if (string.IsNullOrEmpty(text))
            {
                //Show error in ui in case we don't want empty strings
                return true;
            }
            return false;
        }
    }
}