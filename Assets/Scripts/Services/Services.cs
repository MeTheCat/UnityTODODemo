using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameServices
{
    public class Services
    {
        public static Services Instance { get; private set; }
        private Services() { }
        private Dictionary<Type, IService> services = new();

        public static void Init()
        {
            Instance = new Services();
        }

        public T Get<T>() where T : IService
        {
            if (services.TryGetValue(typeof(T), out IService s))
            {
                return (T)s;
            }
            throw new Exception($"[Services] Service of type {typeof(T)} is not found");
        }

        public void Register<T>(T service) where T : IService
        {
            if (!services.TryAdd(typeof(T), service))
            {
                Debug.LogError($"[Services] Service of type {typeof(T)} is already registered");
            }
        }

        public void Unregister<T>()
        {
            if (!services.ContainsKey(typeof(T)))
            {
                Debug.LogError($"[Services] Service of type {typeof(T)} is not found");
                return;
            }

            services.Remove(typeof(T));
        }
    }
}