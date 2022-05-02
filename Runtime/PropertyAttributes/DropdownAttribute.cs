using System;

namespace DreamBuilders
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DropdownAttribute : AttributeDrawer
    {
        #region Fields

        public string ValuesName { get; private set; }

        #endregion

        #region Constructors

        public DropdownAttribute(string valuesName) => ValuesName = valuesName;

        #endregion

        #region Methods

        #endregion
    }
}