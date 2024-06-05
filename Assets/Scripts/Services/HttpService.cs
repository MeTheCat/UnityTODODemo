using UnityEngine;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Text;
using System.Collections.Concurrent;
#if TEST_SLOWHTTP
using System.Threading;
#endif

namespace GameServices
{
    public class HttpService : IService
    {
        private HttpClient httpClient = new();
        private readonly bool isUseCache = false;
        private readonly int cacheTTLMinutes = 3;
        private readonly ConcurrentDictionary<string, CacheItem> cacheDictionary = new();

        private class CacheItem
        {
            public string Data;
            public DateTime Expiration { get; }

            public CacheItem(string data, DateTime expiration)
            {
                Data = data;
                Expiration = expiration;
            }
        }

        public HttpService(bool isUseCache = false, int timeoutSeconds = 10, int cacheTTLMinutes = 3)
        {
            httpClient.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
            this.isUseCache = isUseCache;
            this.cacheTTLMinutes = cacheTTLMinutes;
        }

        public async Task<string> GetRequestAsync(string url)
        {
            if (this.isUseCache && cacheDictionary.TryGetValue(url, out CacheItem cachedItem))
            {
                if (cachedItem.Expiration >= DateTime.Now)
                {
                    return cachedItem.Data;
                }
                else
                {
                    cacheDictionary.TryRemove(url, out _);
                }
            }

            try
            {
                var resp = await httpClient.GetAsync(url);
                resp.EnsureSuccessStatusCode(); //Will throw an exception if status code is not 2xx
                var result = await resp.Content.ReadAsStringAsync();

                if (isUseCache)
                {
                    DateTime expiration = GetCacheExpireTime(resp);
                    cacheDictionary[url] = new CacheItem(result, expiration);
                    Debug.Log($"[HttpService] Cache expire time {expiration} ; Time now: {DateTime.Now}");
                }

                return result;
            }
            catch (Exception e)
            {
                Debug.LogError($"[HttpService] Request to {url} failed with an exception {e.Message}");
                throw;
            }
        }

        public async Task<string> PostRequestAsync(string url, string jsonData)
        {
            try
            {
#if TEST_SLOWHTTP
            await Task.Run(() => { Thread.Sleep(3000); });
#endif
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
#if TEST_SLOWHTTP
            await Task.Run(() => { Thread.Sleep(3000); });
#endif
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

        private DateTime GetCacheExpireTime(HttpResponseMessage resp)
        {
            if (resp.Headers.CacheControl?.MaxAge != null)
            {
                Debug.Log($"MaxAge {resp.Headers.CacheControl.MaxAge.Value.TotalSeconds}");
                return DateTime.Now.Add(resp.Headers.CacheControl.MaxAge.Value);
            }

            if (resp.Content.Headers.Expires != null)
            {
                return resp.Content.Headers.Expires.Value.DateTime;
            }

            return DateTime.Now.AddMinutes(cacheTTLMinutes);
        }
    }
}