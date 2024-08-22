using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Extensions.Core;

namespace Net.Maui.Extensions.ControlFlow;

internal sealed class NavigationService : INavigationService
{
    private readonly ScopedApplicationShell scopedApplicationShell;
    private readonly ILogger<NavigationService> logger;

    public NavigationService(
        ScopedApplicationShell scopedApplicationShell,
        ILogger<NavigationService> logger)
    {
        this.scopedApplicationShell = scopedApplicationShell.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public async Task GoBack(bool animated = false)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogInformation("Going back");
        await this.scopedApplicationShell.Navigation.PopAsync(animated);
    }

    public async Task GoBackModal(bool animated = false)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogInformation("Going back");
        await this.scopedApplicationShell.Navigation.PopModalAsync(animated);
    }

    public async Task GoBackToRoot(bool animated = false)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogInformation("Going back to root");
        await this.scopedApplicationShell.Navigation.PopToRootAsync(animated);
    }

    public async Task GoTo<T>(bool animated = false) where T : ContentPage
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogInformation($"Going to {typeof(T).Name}");
        await this.scopedApplicationShell.PushScoped<T>();
    }

    public async Task GoToModal<T>(bool animated = false) where T : ContentPage
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogInformation($"Going to {typeof(T).Name}");
        await this.scopedApplicationShell.PushModalScoped<T>();
    }
}
