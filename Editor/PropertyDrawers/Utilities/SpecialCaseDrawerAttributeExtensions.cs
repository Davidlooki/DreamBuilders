using System;
using System.Collections.Generic;

namespace DreamBuilders.Editor
{
    public static class SpecialCaseDrawerAttributeExtensions
    {
        #region Fields

        private static readonly Dictionary<Type, SpecialCasePropertyDrawerBase> _drawersByAttributeType;

        #endregion

        #region Constructors

        static SpecialCaseDrawerAttributeExtensions()
        {
            _drawersByAttributeType = new Dictionary<Type, SpecialCasePropertyDrawerBase>
            {
                [typeof(ReorderableListAttribute)] = ReorderableListPropertyDrawer.Instance
            };
        }

        #endregion

        #region Methods

        public static SpecialCasePropertyDrawerBase GetDrawer(this SpecialCaseDrawerAttribute attr) =>
            _drawersByAttributeType.TryGetValue(attr.GetType(), out SpecialCasePropertyDrawerBase drawer)
                ? drawer
                : null;

        #endregion
    }
}