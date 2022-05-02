using System;

namespace DreamBuilders
{
    [AttributeUsage(AttributeTargets.Field)]
    public class BoxGroupAttribute : MetaAttribute, IGroupPropertyAttribute
    {
        #region Fields

        public string Name { get; private set; }

        #endregion

        #region Constructors

        public BoxGroupAttribute(string name = "") => Name = name;

        #endregion

        #region Methods

        #endregion
    }
}