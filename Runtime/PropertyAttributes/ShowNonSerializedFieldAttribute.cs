using System;

namespace DreamBuilders
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ShowNonSerializedFieldAttribute : SpecialCaseDrawerAttribute
    { }
}