using System.ComponentModel;

namespace Net.Maui.Extensions.ControlFlow;

public interface INavigationService
{
    ContentPage? GetCurrent();

    TPageType GoTo<TPageType, TViewModelType>(TViewModelType? viewModel = default)
        where TPageType : ContentPage
        where TViewModelType : INotifyPropertyChanged, new();

    void GoBack();

    void GoBackToRoot();
}
