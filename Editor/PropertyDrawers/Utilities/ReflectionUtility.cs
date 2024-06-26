using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace DreamBuilders.Editor
{
    public static class ReflectionUtility
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Methods

        public static IEnumerable<FieldInfo> GetAllFields(object target, Func<FieldInfo, bool> predicate)
        {
            if (target == null)
            {
                Debug.LogError("The target object is null. Check for missing scripts.");
                yield break;
            }

            List<Type> types = GetSelfAndBaseTypes(target);

            for (int i = types.Count - 1; i >= 0; i--)
            {
                IEnumerable<FieldInfo> fieldInfos = types[i]
                                                    .GetFields(BindingFlags.Instance | BindingFlags.Static |
                                                               BindingFlags.NonPublic | BindingFlags.Public |
                                                               BindingFlags.DeclaredOnly)
                                                    .Where(predicate);

                foreach (FieldInfo fieldInfo in fieldInfos)
                    yield return fieldInfo;
            }
        }

        public static IEnumerable<PropertyInfo> GetAllProperties(object target, Func<PropertyInfo, bool> predicate)
        {
            if (target == null)
            {
                Debug.LogError("The target object is null. Check for missing scripts.");
                yield break;
            }

            List<Type> types = GetSelfAndBaseTypes(target);

            for (int i = types.Count - 1; i >= 0; i--)
            {
                IEnumerable<PropertyInfo> propertyInfos = types[i]
                                                          .GetProperties(BindingFlags.Instance | BindingFlags.Static |
                                                                         BindingFlags.NonPublic | BindingFlags.Public |
                                                                         BindingFlags.DeclaredOnly)
                                                          .Where(predicate);

                foreach (var propertyInfo in propertyInfos)
                    yield return propertyInfo;
            }
        }

        public static IEnumerable<MethodInfo> GetAllMethods(object target, Func<MethodInfo, bool> predicate)
        {
            if (target == null)
            {
                Debug.LogError("The target object is null. Check for missing scripts.");
                yield break;
            }

            List<Type> types = GetSelfAndBaseTypes(target);

            for (int i = types.Count - 1; i >= 0; i--)
            {
                IEnumerable<MethodInfo> methodInfos = types[i]
                                                      .GetMethods(BindingFlags.Instance | BindingFlags.Static |
                                                                  BindingFlags.NonPublic | BindingFlags.Public |
                                                                  BindingFlags.DeclaredOnly)
                                                      .Where(predicate);

                foreach (MethodInfo methodInfo in methodInfos)
                    yield return methodInfo;
            }
        }

        public static FieldInfo GetField(object target, string fieldName) =>
            GetAllFields(target, f =>
                             f.Name.Equals(fieldName, StringComparison.Ordinal)).FirstOrDefault();

        public static PropertyInfo GetProperty(object target, string propertyName) =>
            GetAllProperties(target, p =>
                                 p.Name.Equals(propertyName, StringComparison.Ordinal)).FirstOrDefault();

        public static MethodInfo GetMethod(object target, string methodName) =>
            GetAllMethods(target, m =>
                              m.Name.Equals(methodName, StringComparison.Ordinal)).FirstOrDefault();

        public static Type GetListElementType(Type listType) =>
            listType.IsGenericType ? listType.GetGenericArguments()[0] : listType.GetElementType();

        /// <summary>
        ///		Get type and all base types of target, sorted as following:
        ///		<para />[target's type, base type, base's base type, ...]
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private static List<Type> GetSelfAndBaseTypes(object target)
        {
            List<Type> types = new() {target.GetType()};

            while (types.Last().BaseType != null)
                types.Add(types.Last().BaseType);

            return types;
        }

        #endregion
    }
}