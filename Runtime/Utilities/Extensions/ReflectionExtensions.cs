namespace DreamBuilders
{
    public static class ReflectionExtensions
    {
        public static bool HasMethod(this object target, string methodName)
            => target.GetType().GetMethod(methodName) != null;

        public static bool HasField(this object target, string fieldName)
            => target.GetType().GetField(fieldName) != null;

        public static bool HasProperty(this object target, string propertyName)
            => target.GetType().GetProperty(propertyName) != null;
    }
}