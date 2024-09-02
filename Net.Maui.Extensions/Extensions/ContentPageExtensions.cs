using System.ComponentModel;
using System.Core.Extensions;
using System.Extensions;

namespace Net.Maui.Extensions.Extensions;

public static class ContentPageExtensions
{
    public static T GetBindingContext<T>(this ContentPage contentPage)
        where T : INotifyPropertyChanged
    {
        return contentPage.ThrowIfNull().BindingContext.Cast<T>();
    }
}
