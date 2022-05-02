using UnityEngine;
using System.Reflection;
using System.Collections.Generic;

namespace DreamBuilders.Editor
{
    public static class ButtonUtility
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Methods

        public static bool IsEnabled(Object target, MethodInfo method)
        {
            EnableIfAttributeBase enableIfAttribute = method.GetCustomAttribute<EnableIfAttributeBase>();
            if (enableIfAttribute == null) return true;

            List<bool> conditionValues = PropertyUtility.GetConditionValues(target, enableIfAttribute.Conditions);
            if (conditionValues.Count > 0)
                return PropertyUtility.GetConditionsFlag(conditionValues, enableIfAttribute.ConditionOperator,
                                                         enableIfAttribute.Inverted);

            string message = enableIfAttribute.GetType().Name +
                             " needs a valid boolean condition field, property or method name to work";
            Debug.LogWarning(message, target);

            return false;
        }

        public static bool IsVisible(Object target, MethodInfo method)
        {
            ShowIfAttributeBase showIfAttribute = method.GetCustomAttribute<ShowIfAttributeBase>();
            if (showIfAttribute == null) return true;

            List<bool> conditionValues = PropertyUtility.GetConditionValues(target, showIfAttribute.Conditions);
            if (conditionValues.Count > 0)
                return PropertyUtility.GetConditionsFlag(conditionValues, showIfAttribute.ConditionOperator,
                                                         showIfAttribute.Inverted);

            string message = showIfAttribute.GetType().Name +
                             " needs a valid boolean condition field, property or method name to work";
            Debug.LogWarning(message, target);

            return false;
        }

        #endregion
    }
}