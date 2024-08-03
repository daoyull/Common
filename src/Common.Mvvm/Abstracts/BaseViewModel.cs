using CommunityToolkit.Mvvm.ComponentModel;

namespace Common.Mvvm.Abstracts;

public abstract class BaseViewModel : ObservableObject
{
    public virtual Task OnLoaded()
    {
        return Task.CompletedTask;
    }

    public virtual Task OnUnloaded()
    {
        return Task.CompletedTask;
    }
}