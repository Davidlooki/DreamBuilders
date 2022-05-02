using System;

namespace DreamBuilders
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ButtonAttribute : SpecialCaseDrawerAttribute
    {
        #region Fields

        public string Text { get; private set; }
        public EButtonEnableMode SelectedEnableMode { get; private set; }

        #endregion

        #region Constructors

        public ButtonAttribute(string text = null, EButtonEnableMode enabledMode = EButtonEnableMode.Always) =>
            (Text, SelectedEnableMode) = (text, enabledMode);

        #endregion

        #region Methods

        #endregion
    }
}