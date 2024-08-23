using Net.Maui.Extensions.ControlFlow;
using System.Core.Extensions;
using System.Extensions;

namespace Net.Maui.Extensions;

public abstract class ExtendedApplication : Application
{
    private readonly INavigationService navigationService;

    public ExtendedApplication(
        INavigationService navigationService)
    {
        this.navigationService = navigationService.ThrowIfNull();
        this.SetMainPage();
    }

    private void SetMainPage()
    {
        var currentType = this.GetType();

        // Find the interface that implements IMainPagePresenter<>
        var interfaceType = currentType
            .GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMainPagePresenter<>));

        if (interfaceType is null)
        {
            throw new InvalidOperationException($"{currentType.Name} must implement IMainPagePresenter<TMainPage>");
        }

        var mainPageType = interfaceType.GetGenericArguments()[0];
        this.navigationService.Cast<NavigationService>().SetNavigationRoot(this, mainPageType);
    }
}
