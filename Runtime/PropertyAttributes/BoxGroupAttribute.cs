using System;

namespace DreamBuilders
{
    [AttributeUsage(AttributeTargets.Field)]
    public class BoxGroupAttribute : MetaAttribute, IGroupPropertyAttribute
    {
        public string Name { get; private set; }

        public BoxGroupAttribute(string name = "") => Name = name;
    }
}