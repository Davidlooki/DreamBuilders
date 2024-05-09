using System;

namespace DreamBuilders
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ButtonAttribute : SpecialCaseDrawerAttribute
    {
        public string Text { get; private set; }
        public EButtonEnableMode SelectedEnableMode { get; private set; }

        public ButtonAttribute(string text = null, EButtonEnableMode enabledMode = EButtonEnableMode.Always) =>
            (Text, SelectedEnableMode) = (text, enabledMode);
    }
}