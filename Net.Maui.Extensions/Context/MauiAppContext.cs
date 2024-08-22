namespace Net.Maui.Extensions.Context;

public sealed class MauiAppContext
{
    private readonly Dictionary<string, object?> resources = [];

    internal MauiAppContext()
    {
    }

    public void SetContext<T>(string key, T value)
    {
        this.resources.Add(key, value);
    }

    public bool TryGetContext<T>(string key, out T? value)
    {
        if (this.resources.TryGetValue(key, out var valueObj) &&
            valueObj is T typedValue)
        {
            value = typedValue;
            return true;
        }

        value = default;
        return false;
    }
}
