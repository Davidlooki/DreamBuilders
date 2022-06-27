using System;

namespace DreamBuilders
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class InfoBoxAttribute : AttributeDrawer
    {
        #region Fields

        public string Text { get; private set; }
        public EInfoBoxType Type { get; private set; }

        #endregion

        #region Constructors

        public InfoBoxAttribute(string text, EInfoBoxType type = EInfoBoxType.Normal) =>
            (Text, Type) = (text, type);

        #endregion

        #region Methods

        #endregion
    }
}