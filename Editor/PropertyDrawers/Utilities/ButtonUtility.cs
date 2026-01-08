using UnityEngine;
using System.Reflection;
using System.Collections.Generic;

namespace DreamBuilders.Editor
{
    public static class ButtonUtility
    {
        public static bool IsEnabled(Object target, MethodInfo method)
        {
            EnableIfAttributeBase enableIfAttribute = method.GetCustomAttribute<EnableIfAttributeBase>();
            if (enableIfAttribute == null) return true;

            List<bool> conditionValues = target.GetConditionValues(enableIfAttribute.Conditions);
            if (conditionValues.Count > 0)
                return conditionValues.GetConditionsFlag(enableIfAttribute.ConditionOperator,
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

            List<bool> conditionValues = target.GetConditionValues(showIfAttribute.Conditions);
            if (conditionValues.Count > 0)
                return conditionValues.GetConditionsFlag(showIfAttribute.ConditionOperator,
                                                         showIfAttribute.Inverted);

            string message = showIfAttribute.GetType().Name +
                             " needs a valid boolean condition field, property or method name to work";
            Debug.LogWarning(message, target);

            return false;
        }
    }
}