using System;

namespace DreamBuilders
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method)]
    public class HideIfAttribute : ShowIfAttributeBase
    {
        #region Fields

        #endregion

        #region Constructors

        public HideIfAttribute(string condition) : base(condition) =>
            Inverted = true;

        public HideIfAttribute(EConditionOperator conditionOperator, params string[] conditions) :
            base(conditionOperator, conditions) =>
            Inverted = true;

        public HideIfAttribute(string enumName, object enumValue) : base(enumName, enumValue as Enum) =>
            Inverted = true;

        #endregion

        #region Methods

        #endregion
    }
}