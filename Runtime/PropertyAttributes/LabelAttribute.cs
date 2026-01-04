using System;

namespace DreamBuilders
{
    [AttributeUsage(AttributeTargets.Field)]
    public class LabelAttribute : MetaAttribute
    {
        public string Label { get; private set; }

        public LabelAttribute(string label) => Label = label;
    }
}