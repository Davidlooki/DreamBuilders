namespace DreamBuilders
{
    /// <summary>
    /// <see cref="ButtonAttribute"/> enable options enum.
    /// </summary>
    public enum EButtonEnableMode
    {
        /// <summary>
        /// Button should be active always
        /// </summary>
        Always,

        /// <summary>
        /// Button should be active only in editor
        /// </summary>
        Editor,

        /// <summary>
        /// Button should be active only in playmode
        /// </summary>
        Playmode
    }
}