using UnityEditor;

namespace DreamBuilders.Editor
{
    internal class SavedBool
    {
        #region Fields

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

        #endregion

        #region Constructors

        public SavedBool(string name, bool value) => (_name, _value) = (name, EditorPrefs.GetBool(name, value));

        #endregion

        #region Methods

        #endregion
    }
}