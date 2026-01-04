using System;
using System.Collections.Generic;

namespace DreamBuilders.Editor
{
    public static class SpecialCaseDrawerAttributeExtensions
    {
        private static readonly Dictionary<Type, SpecialCasePropertyDrawerBase> _drawersByAttributeType;

        static SpecialCaseDrawerAttributeExtensions()
        {
            _drawersByAttributeType = new Dictionary<Type, SpecialCasePropertyDrawerBase>
            {
                [typeof(ReorderableListAttribute)] = ReorderableListPropertyDrawer.Instance
            };
        }

        public static SpecialCasePropertyDrawerBase GetDrawer(this SpecialCaseDrawerAttribute attr) =>
            _drawersByAttributeType.TryGetValue(attr.GetType(), out SpecialCasePropertyDrawerBase drawer)
                ? drawer
                : null;
    }
}