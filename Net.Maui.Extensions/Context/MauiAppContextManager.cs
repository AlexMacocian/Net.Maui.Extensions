namespace Net.Maui.Extensions.Context;

internal sealed class MauiAppContextManager : IMauiAppContextAccessor
{
    public MauiAppContext Context { get; } = new();
}
