using System;

namespace DreamBuilders
{
    public class ShowIfAttributeBase : MetaAttribute
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

        public ShowIfAttributeBase(string condition) =>
            (ConditionOperator, Conditions) = (EConditionOperator.And, new[] {condition});

        public ShowIfAttributeBase(EConditionOperator conditionOperator, params string[] conditions) =>
            (ConditionOperator, Conditions) = (conditionOperator, conditions);

        public ShowIfAttributeBase(string enumName, Enum enumValue) : this(enumName) =>
            EnumValue = enumValue ??
                        throw new ArgumentNullException(nameof(enumValue),
                                                        "This parameter must be an enum value.");

        #endregion

        #region Methods

        #endregion
    }
}