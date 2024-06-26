﻿using UnityEngine;
using UnityEditor;

namespace DreamBuilders.Editor
{
    [CustomPropertyDrawer(typeof(HorizontalLineAttribute))]
    public class HorizontalLineDecoratorDrawer : DecoratorDrawer
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Methods

        public override float GetHeight()
        {
            HorizontalLineAttribute lineAttr = (HorizontalLineAttribute) attribute;
            return EditorGUIUtility.singleLineHeight + lineAttr.Height;
        }

        public override void OnGUI(Rect position)
        {
            Rect rect = EditorGUI.IndentedRect(position);
            rect.y += EditorGUIUtility.singleLineHeight / 3.0f;
            HorizontalLineAttribute lineAttr = (HorizontalLineAttribute) attribute;
            DreamBuildersEditorGUI.HorizontalLine(rect, lineAttr.Height, lineAttr.Color.GetColor());
        }

        #endregion
    }
}