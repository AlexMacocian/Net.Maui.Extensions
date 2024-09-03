using Net.Maui.Extensions.ControlFlow;
using System.ComponentModel;
using System.Core.Extensions;
using System.Extensions;

namespace Net.Maui.Extensions.Extensions;

public static class ContentPageExtensions
{
    public static T? GetBindingContext<T>(this IPageViewModel<T> page)
        where T : class, INotifyPropertyChanged, new()
    {
        return page.As<BindableObject>()?.ThrowIfNull()?.BindingContext.As<T>();
    }
}
