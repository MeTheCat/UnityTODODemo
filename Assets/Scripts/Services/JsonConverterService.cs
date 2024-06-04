using System;
using Newtonsoft.Json;
using UnityEngine;

namespace GameServices
{
    public class JsonConverterService : IService
    {
        public T DeserializeObject<T>(string jsonData)
        {
            try
            {
                var obj = JsonConvert.DeserializeObject<T>(jsonData);
                return obj;
            }
            catch (Exception e)
            {
                Debug.LogError($"[JsonConverterService] Deserialize failed with an exception {e.Message}");
                throw;
            }
        }

        public string SerializeObject<T>(T obj)
        {
            try
            {
                string jsonData = JsonConvert.SerializeObject(obj);
                return jsonData;
            }
            catch (Exception e)
            {
                Debug.LogError($"[JsonConverterService] Serialize failed with an exception {e.Message}");
                throw;
            }
        }
    }
}