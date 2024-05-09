using System;

namespace DreamBuilders
{
    [AttributeUsage(AttributeTargets.Field)]
    public class FoldoutAttribute : MetaAttribute, IGroupPropertyAttribute
    {
        public string Name { get; private set; }

        public FoldoutAttribute(string name) => Name = name;
    }
}