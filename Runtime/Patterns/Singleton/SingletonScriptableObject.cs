﻿using System.Linq;
using UnityEngine;

namespace DreamBuilders
{
    /// <summary>
    /// Abstract class for making reload-proof singletons out of ScriptableObjects
    /// Returns the asset created on the editor, or null if there is none
    /// Based on https://www.youtube.com/watch?v=VBA1QCoEAX4
    /// </summary>
    /// <typeparam name="T">Singleton type</typeparam>
    public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
    {
        static T _instance = null;
        public static T Instance
        {
            get
            {
                if (!_instance)
                    _instance = GetSO();

                return _instance;
            }
        }

        private static T GetSO()
        {
            T so = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
            if (!so)
                so = CreateInstance<T>();
            return so;
        }
    }
}