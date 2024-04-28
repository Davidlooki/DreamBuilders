using System;
using System.Collections.Generic;
using UnityEngine;

namespace DreamBuilders
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<string, IService> _services = new();

        public static void Register<T>(T service) where T : class, IService
        {
            if (service == null)
                return;

            string serviceName = GetServiceName<T>();

            if (!string.IsNullOrWhiteSpace(serviceName) && !_services.TryAdd(serviceName, service))
                Debug.Log($"[{nameof(ServiceLocator)}] \"{serviceName}\" is already registered!");
        }

        public static void Unregister<T>() where T : class, IService
        {
            string serviceName = GetServiceName<T>();

            if (string.IsNullOrWhiteSpace(serviceName) || !_services.Remove(serviceName, out IService service) ||
                service == null)
            {
                Debug.Log($"[{nameof(ServiceLocator)}] Could not Unregister \"{serviceName}\". Service not found!");

                return;
            }

            if (service is IDisposable disposable)
                disposable.Dispose();
        }

        public static bool TryGet<T>(out T service) where T : class, IService
        {
            string serviceName = GetServiceName<T>();

            if (string.IsNullOrWhiteSpace(serviceName) || !_services.TryGetValue(serviceName, out var foundService))
            {
                Debug.Log($"[{nameof(ServiceLocator)}] Could not locate service \"{serviceName}\"");
                service = null;

                return false;
            }

            service = foundService as T;

            return true;
        }

        private static string GetServiceName<T>() =>
            typeof(T).FullName;

        public static T Get<T>() where T : class, IService =>
            TryGet(out T service) ? service : null;
    }
}