using System;

namespace DreamBuilders
{
    [AttributeUsage(AttributeTargets.Field)]
    public class FoldoutAttribute : MetaAttribute, IGroupPropertyAttribute
    {
        #region Fields

        public string Name { get; private set; }

        #endregion

        #region Constructors

        public FoldoutAttribute(string name) => Name = name;

        #endregion

        #region Methods

        #endregion
    }
}