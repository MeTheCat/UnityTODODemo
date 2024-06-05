using UnityEngine;

namespace Game
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private ServiceLoader serviceLoader;

        [SerializeField] private Tasks.TasksController tasksController;

        private void Start()
        {
            Debug.Log("Game start");
            serviceLoader.Init();
            tasksController.Init();
        }
    }
}
