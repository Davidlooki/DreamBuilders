using System.Collections;
using System.Collections.Generic;

namespace DreamBuilders
{
    public class DropdownList<T> : IDropdownList
    {
        private readonly List<KeyValuePair<string, object>> _values = new();

        public void Add(string displayName, T value) =>
            _values.Add(new KeyValuePair<string, object>(displayName, value));

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => _values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static explicit operator DropdownList<object>(DropdownList<T> target)
        {
            DropdownList<object> result = new();
            foreach ((string key, object value) in target)
                result.Add(key, value);

            return result;
        }
    }
}