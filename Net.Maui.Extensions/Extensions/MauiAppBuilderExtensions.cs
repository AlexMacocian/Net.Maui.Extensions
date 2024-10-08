﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Net.Maui.Extensions.Attributes;
using Net.Maui.Extensions.Context;
using Net.Maui.Extensions.ControlFlow;
using System.Core.Extensions;
using System.Extensions;
using System.Reflection;

namespace Net.Maui.Extensions;

public static class MauiAppBuilderExtensions
{
    private static readonly Lazy<MethodInfo> ConfigureMethod = new(() =>
    {
        return typeof(OptionsConfigurationServiceCollectionExtensions)?
                .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .FirstOrDefault(m => m.Name == nameof(OptionsConfigurationServiceCollectionExtensions.Configure)
                                  && m.GetGenericArguments().Length == 1
                                  && m.GetParameters().Length == 2
                                  && m.GetParameters()[0].ParameterType == typeof(IServiceCollection)
                                  && m.GetParameters()[1].ParameterType == typeof(IConfiguration)) ?? throw new InvalidOperationException($"Unable to resolve {nameof(OptionsConfigurationServiceCollectionExtensions.Configure)} method");
    }, true);

    public static MauiAppBuilder WithExtendedOptions(this MauiAppBuilder appBuilder)
    {
        appBuilder.ThrowIfNull();
        foreach ((var optionType, var optionAttribute) in Assembly.GetCallingAssembly()?.GetTypes()?
            .Select(t => (t, t.GetCustomAttributes(true).OfType<OptionSectionAttribute>().FirstOrDefault()))
            .Where(t => t.Item2 is not null) ?? [])
        {
            var section = appBuilder.Configuration.GetRequiredSection(optionAttribute?.SectionName ?? optionType.Name);
            // Use reflection to call the generic Configure<T> method
            var method = ConfigureMethod.Value
                .MakeGenericMethod(optionType);

            method?.Invoke(null, [appBuilder.Services, section]);
        }

        return appBuilder;
    }

    public static MauiAppBuilder WithMauiAppContext(this MauiAppBuilder appBuilder)
    {
        appBuilder.ThrowIfNull()
            .Services.AddScoped<IMauiAppContextAccessor, MauiAppContextManager>();

        return appBuilder;
    }

    public static MauiAppBuilder WithExtendedApp<TApp, TMainPage>(this MauiAppBuilder appBuilder)
        where TApp : ExtendedApplication, IMainPagePresenter<TMainPage>
        where TMainPage : ContentPage
    {
        appBuilder.ThrowIfNull().UseMauiApp<TApp>();
        return appBuilder;
    }

    public static MauiAppBuilder WithNavigation(this MauiAppBuilder appBuilder)
    {
        appBuilder.ThrowIfNull();
        appBuilder.Services.AddSingleton<INavigationService, NavigationService>(sp => new NavigationService(sp, sp.GetRequiredService<ILogger<NavigationService>>()));

        return appBuilder;
    }

    public static MauiAppBuilder WithPages(this MauiAppBuilder appBuilder)
    {
        appBuilder.ThrowIfNull();
        foreach (var pageType in Assembly.GetCallingAssembly()?.GetTypes()?.Where(t => t.IsAssignableTo(typeof(ContentPage)) && !t.IsAbstract && !t.IsInterface) ?? [])
        {
            appBuilder.Services.AddScoped(pageType);
            appBuilder.Services.AddScoped(typeof(ContentPage), sp => sp.GetRequiredService(pageType));
        }

        return appBuilder;
    }
}
