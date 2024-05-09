using UnityEditor;

namespace DreamBuilders.Editor
{
    internal class SavedBool
    {
        private bool _value;
        private readonly string _name;

        public bool Value
        {
            get => _value;
            set
            {
                if (_value == value) return;

                _value = value;
                EditorPrefs.SetBool(_name, value);
            }
        }

        public SavedBool(string name, bool value) => (_name, _value) = (name, EditorPrefs.GetBool(name, value));
    }
}