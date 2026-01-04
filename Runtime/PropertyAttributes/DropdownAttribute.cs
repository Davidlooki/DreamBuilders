using System;

namespace DreamBuilders
{
    /// <summary>
    /// Allow the selection of a value on Inspector through a dropdown list.
    /// </summary>
    /// <remarks>
    /// Mind that struct's nested values will not update. Avoid using on this context.
    /// </remarks>
    /// <example>
    /// <code>
    /// public class DropdownExample : MonoBehaviour
    /// {
    ///     [Dropdown(nameof(_intValues))]
    ///     public int IntValue;
    ///      
    ///     private int[] _intValues = new int[] { 1, 2, 3, 4, 5 };
    ///     
    ///     [Dropdown(nameof(_stringValues))]
    ///     public string StringValue;
    ///     
    ///     private string[] _stringValues => new string[] {"a","b","c"}
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Field)]
    public class DropdownAttribute : AttributeDrawer
    {
        public string ValuesName { get; private set; }

        public DropdownAttribute(string valuesName) => ValuesName = valuesName;
    }
}