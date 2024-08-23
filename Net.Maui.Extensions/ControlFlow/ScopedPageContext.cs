namespace Net.Maui.Extensions.ControlFlow;

internal sealed class ScopedPageContext
{
    public ContentPage? Page { get; init; }
    public IServiceScope? Scope { get; init; }
}
