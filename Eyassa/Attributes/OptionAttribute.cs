namespace Eyassa.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class OptionAttribute(string customId) : Attribute
{
    public string CustomId { get; } = customId;
}