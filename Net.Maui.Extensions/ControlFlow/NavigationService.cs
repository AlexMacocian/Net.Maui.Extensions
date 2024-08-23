﻿using Microsoft.Extensions.Logging;
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

    public void GoBack(bool animated = false)
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

    public void GoBackToRoot(bool animated = false)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogDebug("Going back to root");
        while (this.navigationStack.Count > 1 && this.navigationStack.TryPop(out var context))
        {
            context.Scope?.Dispose();
        }

        this.ShowCurrentPage();
    }

    public void GoTo<T>(bool animated = false) where T : ContentPage
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogDebug($"Going to {typeof(T).Name}");

        var context = this.GetScopedPageAndProvider<T>();
        this.navigationStack.Push(context);
        this.ShowCurrentPage();
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

        if (this.navigationStack.TryPeek(out var currentContext))
        {
            this.extendedApplication.MainPage = currentContext.Page;
        }
    }

    private ScopedPageContext GetScopedPageAndProvider<T>()
        where T : ContentPage
    {
        var scope = this.serviceProvider.CreateScope();
        return new ScopedPageContext { Page = scope.ServiceProvider.GetRequiredService<T>(), Scope = scope };
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
