using System;

namespace DreamBuilders
{
    [AttributeUsage(AttributeTargets.Field)]
    public class LabelAttribute : MetaAttribute
    {
        #region Fields

        public string Label { get; private set; }

        #endregion

        #region Constructors

        public LabelAttribute(string label) => Label = label;

        #endregion

        #region Methods

        #endregion
    }
}