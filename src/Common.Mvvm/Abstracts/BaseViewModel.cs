using CommunityToolkit.Mvvm.ComponentModel;

namespace Common.Mvvm.Abstracts;

public abstract class BaseViewModel : ObservableObject
{
    public abstract Task RefreshUi();
    public virtual async Task OnLoaded()
    {
        await RefreshUi();
    }

    public virtual Task OnUnloaded()
    {
        return Task.CompletedTask;
    }
}