using System.ComponentModel;

namespace Net.Maui.Extensions.ControlFlow;

public interface INavigationService
{
    ContentPage? GetCurrent();

    TPageType GoTo<TPageType, TViewModelType>(TViewModelType? viewModel = default)
        where TPageType : ContentPage, IPageViewModel<TViewModelType>
        where TViewModelType : class, INotifyPropertyChanged, new();

    void GoBack();

    void GoBackTo<TPageType>()
        where TPageType : ContentPage;

    void GoBackToRoot();
}
