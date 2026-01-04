using System;

namespace DreamBuilders
{
    /// <summary>
    /// Marks a method with a clickable button on Inspector.
    /// </summary>
    /// <remarks>
    /// Also works with default parameters methods and coroutines.
    /// Use <see cref="EButtonEnableMode"/> to set preferences between Editor, Playmode or Always(both).
    /// </remarks>
    /// <example>
    /// <code>
    /// public class ButtonExample : MonoBehaviour
    /// {
    ///     [Button]
    ///     private void Foo() { }
    /// 
    ///     [Button]
    ///     private void DefaultParametersMethod(int parameter = 0) { }
    ///     
    ///     [Button("Button Text")]
    ///     private void CustomButtonTextMethod() { }
    ///     
    ///     [Button(enabledMode: EButtonEnableMode.Editor)]
    ///     private void EnabledButtonInEditorOnly() { }
    ///     
    ///     [Button(enabledMode: EButtonEnableMode.Playmode)]
    ///     private void EnabledButtonInPlaymodeOnly() { }
    ///     
    ///     [Button]
    ///     private IEnumerator CoroutineMethod() { }
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Method)]
    public class ButtonAttribute : SpecialCaseDrawerAttribute
    {
        public string Text { get; private set; }
        public EButtonEnableMode SelectedEnableMode { get; private set; }

        public ButtonAttribute(string text = null, EButtonEnableMode enabledMode = EButtonEnableMode.Always) =>
            (Text, SelectedEnableMode) = (text, enabledMode);
    }
}