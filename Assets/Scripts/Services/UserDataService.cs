using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Data;
using GameServices;
using UnityEngine;

public class UserDataService : IService
{
    public class UserData
    {
        //public List<TaskItem> TaskItems = new();
        public Dictionary<int, TaskItem> TaskItemsById;
        public int GetLastId() => TaskItemsById.Max(x => x.Key);
    }
    public UserData GetUserData() => userData;
    private UserData userData = new();
    private HttpService httpService;
    private JsonConverterService jsonConverterService;
    private readonly string todoUrl = "https://jsonplaceholder.typicode.com/todos/";

    public UserDataService()
    {
        httpService = Services.Instance.Get<HttpService>();
        jsonConverterService = Services.Instance.Get<JsonConverterService>();
    }

    public async Task FetchInitialDataAsync()
    {
        try
        {
            //await Task.Run(() => { Thread.Sleep(5000); });
            var res = await httpService.GetRequestAsync(todoUrl);
            var data = jsonConverterService.DeserializeObject<List<TaskItem>>(res);

            userData.TaskItemsById = data.ToDictionary(data => data.id);
        }
        catch (Exception e)
        {
            Debug.LogError($"[UserDataService] Error retrieving tasks list {e.Message}");
            throw;
        }
    }

    public async Task<Data.TaskItem> AddTaskAsync(string text)
    {
        try
        {
            int nextId = this.userData.GetLastId() + 1; //Dummy backend doesn't actually return new id on each add

            string jsonData = "";
            var newItemData = new Data.TaskItem()
            {
                title = HttpUtility.UrlEncode(text),
                id = nextId
            };
            jsonData = jsonConverterService.SerializeObject(newItemData);

            //await Task.Run(() => { Thread.Sleep(2000); });
            var resp = await httpService.PostRequestAsync(todoUrl, jsonData);
            Debug.Log($"[UserDataService] Add new task result: {resp} with id {nextId}");

            var newItem = jsonConverterService.DeserializeObject<Data.TaskItem>(resp);
            if (newItem.id != nextId) Debug.Log($"[UserDataService] AddItem Id mismatch, expected {nextId}, got: {newItem.id}"); //warn
            this.userData.TaskItemsById[nextId] = newItem;
            return newItem;
        }
        catch (Exception e)
        {
            Debug.LogError($"[UserDataService] Error adding task {e.Message}");
            throw;
        }
    }

    public async Task EditTaskAsync(int id, string text)
    {
        try
        {
            string jsonData = "";
            var newItemData = new Data.TaskItem()
            {
                id = id,
                title = HttpUtility.UrlEncode(text)
            };
            jsonData = jsonConverterService.SerializeObject(newItemData);

            var resp = await httpService.PutRequestAsync(string.Concat(todoUrl, id.ToString()), jsonData);
            Debug.Log($"[UserDataService] Edit task result: {resp} - updated id {id} with content {jsonData}");

            this.userData.TaskItemsById[id].title = text;
        }
        catch (Exception e)
        {
            //There will be an exception when trying to edit an item with id >= 201
            Debug.LogError($"[UserDataService] Error editing task {e.Message}");
            throw;
        }
    }
}
