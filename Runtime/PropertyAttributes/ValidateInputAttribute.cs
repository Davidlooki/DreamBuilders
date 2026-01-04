using System;

namespace DreamBuilders
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ValidateInputAttribute : ValidatorAttribute
    {
        public string CallbackName { get; private set; }
        public string Message { get; private set; }

        public ValidateInputAttribute(string callbackName, string message = null) =>
            (CallbackName, Message) = (callbackName, message);
    }
}