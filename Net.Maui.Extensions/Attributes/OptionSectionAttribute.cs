namespace Net.Maui.Extensions.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class OptionSectionAttribute : Attribute
{
    public string? SectionName { get; set; }
}
