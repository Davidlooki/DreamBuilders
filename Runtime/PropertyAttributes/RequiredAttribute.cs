using System;

namespace DreamBuilders
{
    [AttributeUsage(AttributeTargets.Field)]
    public class RequiredAttribute : ValidatorAttribute
    {
        public string Message { get; private set; }

        public RequiredAttribute(string message = null) => Message = message;
    }
}