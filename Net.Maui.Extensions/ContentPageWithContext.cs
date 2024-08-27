using System.Extensions;
using System.Runtime.CompilerServices;

namespace Net.Maui.Extensions;

public abstract class ContentPageWithContext<T> : ContentPage
{
	public new T BindingContext
	{
		get => base.BindingContext.Cast<T>();
		set => base.BindingContext = value;
	}

    protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        if (propertyName == BindingContextProperty.PropertyName)
        {
            if (base.BindingContext is not T)
            {
                throw new InvalidOperationException($"Binding context has been set to a value of type {base.BindingContext.GetType().Name}. Only values of type {typeof(T).Name}");
            }

        }

        base.OnPropertyChanged(propertyName);
    }
}