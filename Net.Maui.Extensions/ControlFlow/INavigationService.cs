namespace Net.Maui.Extensions.ControlFlow;

public interface INavigationService
{
    void GoTo<T>(bool animated = false)
        where T : ContentPage;

    void GoBack(bool animated = false);

    void GoBackToRoot(bool animated = false);
}
