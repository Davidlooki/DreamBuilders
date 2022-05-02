using System;

namespace DreamBuilders
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class OnValueChangedAttribute : MetaAttribute
    {
        #region Fields

        public string CallbackName { get; private set; }

        #endregion

        #region Constructors

        public OnValueChangedAttribute(string callbackName) => CallbackName = callbackName;

        #endregion

        #region Methods

        #endregion
    }
}