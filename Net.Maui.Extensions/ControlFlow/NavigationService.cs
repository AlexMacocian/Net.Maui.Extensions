using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Core.Extensions;
using System.Extensions;
using System.Extensions.Core;

namespace Net.Maui.Extensions.ControlFlow;

internal sealed class NavigationService : INavigationService
{
    private readonly Stack<ScopedPageContext> modalStack = new();
    private readonly Stack<ScopedPageContext> navigationStack = new();
    private readonly IServiceProvider serviceProvider;
    private readonly ILogger<NavigationService> logger;

    private ExtendedApplication? extendedApplication;

    public NavigationService(
        IServiceProvider serviceProvider,
        ILogger<NavigationService> logger)
    {
        this.serviceProvider = serviceProvider.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public void GoBack()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (this.navigationStack.Count <= 1)
        {
            scopedLogger.LogError("Cannot go back. Navigation stack is at root");
            return;
        }

        scopedLogger.LogDebug("Going back");
        if (this.navigationStack.TryPop(out var context))
        {
            context.Scope?.Dispose();
        }

        this.ShowCurrentPage();
    }

    public void GoBackToRoot()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogDebug("Going back to root");
        while (this.navigationStack.Count > 1 && this.navigationStack.TryPop(out var context))
        {
            context.Scope?.Dispose();
        }

        this.ShowCurrentPage();
    }

    public TPageType GoTo<TPageType, TViewModelType>(TViewModelType? viewModel = default)
        where TPageType : ContentPage
        where TViewModelType : INotifyPropertyChanged, new()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogDebug($"Going to {typeof(TPageType).Name}");

        var context = this.GetScopedPageAndProvider<TPageType, TViewModelType>(viewModel ?? new TViewModelType());
        this.navigationStack.Push(context);
        this.ShowCurrentPage();
        return context.Page?.ThrowIfNull().Cast<TPageType>().ThrowIfNull()!;
    }

    public ContentPage? GetCurrent()
    {
        if (this.navigationStack.TryPeek(out var context))
        {
            return context.Page?.ThrowIfNull();
        }

        return default;
    }

    public void SetNavigationRoot(ExtendedApplication extendedApplication, Type rootPageType)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogDebug($"Setting navigation root {rootPageType.Name}");

        this.extendedApplication = extendedApplication.ThrowIfNull();
        var context = this.GetScopedPageAndProvider(rootPageType);
        this.navigationStack.Push(context);
        this.ShowCurrentPage();
    }

    private void ShowCurrentPage()
    {
        if (this.extendedApplication is null)
        {
            return;
        }

        if (this.navigationStack.TryPeek(out var currentContext) &&
            currentContext.Page is not null)
        {
            currentContext.Page.BindingContext = currentContext.ViewModel;
            this.extendedApplication.MainPage = currentContext.Page;
        }
    }

    private ScopedPageContext GetScopedPageAndProvider<TPageType, TViewModelType>(TViewModelType viewModel)
        where TPageType : ContentPage
        where TViewModelType : INotifyPropertyChanged, new()
    {
        var scope = this.serviceProvider.CreateScope();
        return new ScopedPageContext { Page = scope.ServiceProvider.GetRequiredService<TPageType>(), Scope = scope, ViewModel = viewModel };
    }

    private ScopedPageContext GetScopedPageAndProvider(Type type)
    {
        if (!type.IsAssignableTo(typeof(ContentPage)))
        {
            throw new InvalidOperationException($"{type.Name} must be of type {nameof(ContentPage)}");
        }

        var scope = this.serviceProvider.CreateScope();
        return new ScopedPageContext { Page = scope.ServiceProvider.GetRequiredService(type).Cast<ContentPage>(), Scope = scope };
    }
}
