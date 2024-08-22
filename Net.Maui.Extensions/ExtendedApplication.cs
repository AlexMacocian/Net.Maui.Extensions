using Net.Maui.Extensions.ControlFlow;
using System.Core.Extensions;

namespace Net.Maui.Extensions;

public abstract class ExtendedApplication : Application
{
    public ScopedApplicationShell ScopedShell { get; }

    public ExtendedApplication(ScopedApplicationShell shell)
    {
        if (shell is not ScopedApplicationShell)
        {
            throw new InvalidOperationException($"Shell is not of type {typeof(ScopedApplicationShell).Name}");
        }

        this.MainPage = shell.ThrowIfNull();
        this.ScopedShell = shell.ThrowIfNull();
    }
}
