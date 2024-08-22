using System.Core.Extensions;

namespace Net.Maui.Extensions;

public abstract class ExtendedApplication : Application
{
    public ExtendedApplication(Shell shell)
    {
        this.MainPage = shell.ThrowIfNull();
    }
}
