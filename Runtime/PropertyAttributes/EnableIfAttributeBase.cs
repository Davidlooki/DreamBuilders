using System;

namespace DreamBuilders
{
    public abstract class EnableIfAttributeBase : MetaAttribute
    {
        #region Fields

        public string[] Conditions { get; private set; }
        public EConditionOperator ConditionOperator { get; private set; }
        public bool Inverted { get; protected set; }

        /// <summary>
        ///		If this not null, <see cref="Conditions"/>[0] is name of an enum variable.
        /// </summary>
        public Enum EnumValue { get; private set; }

        #endregion

        #region Constructors

        public EnableIfAttributeBase(string condition) =>
            (ConditionOperator, Conditions) = (EConditionOperator.And, new string[1] {condition});

        public EnableIfAttributeBase(EConditionOperator conditionOperator, params string[] conditions) =>
            (ConditionOperator, Conditions) = (conditionOperator, conditions);

        public EnableIfAttributeBase(string enumName, Enum enumValue) : this(enumName) =>
            EnumValue = enumValue ??
                        throw new ArgumentNullException(nameof(enumValue), "This parameter must be an enum value.");

        #endregion

        #region Methods

        #endregion
    }
}