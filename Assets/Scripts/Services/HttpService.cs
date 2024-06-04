using UnityEngine;
using System.Net.Http;
//using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using System;
using System.Text;

namespace GameServices
{
    public class HttpService : IService
    {
        private HttpClient httpClient = new();
        //private IMemoryCache _cache;

        public HttpService()
        {
            // HttpRequestCachePolicy policy = new HttpRequestCachePolicy(HttpRequestCacheLevel.Default);
            // HttpWebRequest.DefaultCachePolicy = policy;
        }

        public async Task<string> GetRequestAsync(string url)
        {
            try
            {
                var resp = await httpClient.GetAsync(url);
                resp.EnsureSuccessStatusCode(); //Will throw an exception if status code is not 2xx
                var result = await resp.Content.ReadAsStringAsync();
                return result;
            }
            catch (Exception e)
            {
                Debug.LogError($"[HttpService] Request to {url} failed with an exception {e.Message}");
                throw;
            }
        }

        public async Task DeleteRequestAsync(string url)
        {
            try
            {
                var resp = await httpClient.DeleteAsync(url);
                resp.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
                Debug.LogError($"[HttpService] Request to {url} failed with an exception {e.Message}");
                throw;
            }
        }

        public async Task<string> PostRequestAsync(string url, string jsonData)
        {
            try
            {
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
            catch (HttpRequestException e)
            {
                Debug.LogError($"[HttpService] Request to {url} failed with an exception {e.Message}");
                throw;
            }
        }

        public async Task<string> PutRequestAsync(string url, string dataJSON)
        {
            try
            {
                HttpContent content = new StringContent(dataJSON, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PutAsync(url, content);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
            catch (HttpRequestException e)
            {
                Debug.LogError($"[HttpService] Request to {url} failed with an exception {e.Message}");
                throw;
            }
        }

        public void CancelPendingRequests()
        {
            try
            {
                this.httpClient.CancelPendingRequests();
            }
            catch (Exception e)
            {
                Debug.LogError($"[HttpService] Failed to cancel pending requests {e}");
                throw;
            }
        }
    }
}