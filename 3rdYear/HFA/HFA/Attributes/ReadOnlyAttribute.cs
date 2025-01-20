namespace HFA.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ReadOnlyAttribute(bool isReadOnly) : Attribute
    {
        public bool IsReadOnly { get; private set; } = isReadOnly;
    }
}