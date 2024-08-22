using System.Core.Extensions;

namespace Net.Maui.Extensions.ControlFlow;

internal class ScopedApplicationShell : Shell
{
    private readonly IServiceProvider serviceProvider;

    public ScopedApplicationShell(
        IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider.ThrowIfNull();
    }

    public async Task PushScoped<T>()
        where T : ContentPage
    {
        var page = this.GetScopedPage<T>();
        await this.Navigation.PushAsync(page);
    }

    public async Task PushModalScoped<T>()
        where T : ContentPage
    {
        var page = this.GetScopedPage<T>();
        await this.Navigation.PushModalAsync(page);
    }

    private T GetScopedPage<T>()
        where T : ContentPage
    {
        using var scope = this.serviceProvider.CreateScope();
        return scope.ServiceProvider.GetRequiredService<T>();
    }
}
