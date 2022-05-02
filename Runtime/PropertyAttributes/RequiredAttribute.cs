using System;

namespace DreamBuilders
{
    [AttributeUsage(AttributeTargets.Field)]
    public class RequiredAttribute : ValidatorAttribute
    {
        #region Fields

        public string Message { get; private set; }

        #endregion

        #region Constructors

        public RequiredAttribute(string message = null) => Message = message;

        #endregion

        #region Methods

        #endregion
    }
}