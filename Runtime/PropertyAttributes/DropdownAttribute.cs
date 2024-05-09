using System;

namespace DreamBuilders
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DropdownAttribute : AttributeDrawer
    {
        public string ValuesName { get; private set; }

        public DropdownAttribute(string valuesName) => ValuesName = valuesName;
    }
}