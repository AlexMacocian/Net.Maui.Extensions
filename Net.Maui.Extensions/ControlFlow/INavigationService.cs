namespace Net.Maui.Extensions.ControlFlow;

public interface INavigationService
{
    ContentPage? GetCurrent();

    T GoTo<T>()
        where T : ContentPage;

    void GoBack();

    void GoBackToRoot();
}
