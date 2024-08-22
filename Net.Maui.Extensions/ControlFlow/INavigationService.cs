namespace Net.Maui.Extensions.ControlFlow;

public interface INavigationService
{
    Task GoTo<T>(bool animated = false)
        where T : ContentPage;

    Task GoToModal<T>(bool animated = false)
        where T : ContentPage;

    Task GoBack(bool animated = false);

    Task GoBackModal(bool animated = false);

    Task GoBackToRoot(bool animated = false);
}
