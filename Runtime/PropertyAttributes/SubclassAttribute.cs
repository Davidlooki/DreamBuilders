using System;
using UnityEngine;

namespace DreamBuilders
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class SubclassAttribute : AttributeDrawer
    {
        public Type BaseType { get; }
        public string NamespacePrefix { get; }

        public SubclassAttribute() { }
        public SubclassAttribute(Type baseType) => BaseType = baseType;
        public SubclassAttribute(string namespacePrefix) => NamespacePrefix = namespacePrefix;
        public SubclassAttribute(Type baseType, string namespacePrefix)
        {
            BaseType = baseType;
            NamespacePrefix = namespacePrefix;
        }
    }
}
