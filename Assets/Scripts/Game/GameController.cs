using UnityEngine;

namespace Game
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private ServiceLoader serviceLoader;

        [SerializeField] private TasksController tasksController;

        private void Start()
        {
            Debug.Log("Game start");
            serviceLoader.Init();

            //Test();

            tasksController.Init();
        }


        // private async void Test()
        // {
        //     var httpService = Services.Instance.Get<HttpService>();
        //     var jsonConvert = Services.Instance.Get<JsonConverterService>();

        //     var res = await httpService.GetRequestAsync("https://jsonplaceholder.typicode.com/todos/");
        //     Debug.Log($"RESULT {res}");

        //     var taskList = jsonConvert.DeserializeObject<List<TaskItem>>(res);
        //     Debug.Log("Total tasks: " + taskList.Count);
        // }
    }
}
