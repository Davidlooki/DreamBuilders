using System;

namespace DreamBuilders
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ValidateInputAttribute : ValidatorAttribute
    {
        #region Fields

        public string CallbackName { get; private set; }
        public string Message { get; private set; }

        #endregion

        #region Constructors

        public ValidateInputAttribute(string callbackName, string message = null) =>
            (CallbackName, Message) = (callbackName, message);

        #endregion

        #region Methods

        #endregion
    }
}