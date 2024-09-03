using System.ComponentModel;

namespace Net.Maui.Extensions.ControlFlow;

public interface IPageViewModel<TViewModelType>
    where TViewModelType : class, INotifyPropertyChanged, new()
{
}
