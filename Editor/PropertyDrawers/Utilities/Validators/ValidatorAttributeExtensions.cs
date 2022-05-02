using System;
using System.Collections.Generic;

namespace DreamBuilders.Editor
{
    public static class ValidatorAttributeExtensions
    {
        #region Fields

        private static readonly Dictionary<Type, PropertyValidatorBase> _validatorsByAttributeType;

        #endregion

        #region Constructors

        static ValidatorAttributeExtensions()
        {
            _validatorsByAttributeType = new Dictionary<Type, PropertyValidatorBase>
            {
                [typeof(MinValueAttribute)] = new MinValuePropertyValidator(),
                [typeof(MaxValueAttribute)] = new MaxValuePropertyValidator(),
                [typeof(RequiredAttribute)] = new RequiredPropertyValidator(),
                [typeof(ValidateInputAttribute)] = new ValidateInputPropertyValidator()
            };
        }

        #endregion

        #region Methods

        public static PropertyValidatorBase GetValidator(this ValidatorAttribute attr) =>
            _validatorsByAttributeType.TryGetValue(attr.GetType(), out PropertyValidatorBase validator)
                ? validator
                : null;

        #endregion
    }
}