using Common.Lib.Service;

namespace Common.Lib.Plugins;

public class RefreshPlugin : ILifePlugin
{
    public Task OnCreated(ILifeCycle lifeCycle)
    {
        return Task.CompletedTask;
    }

    public Task OnLoaded(ILifeCycle lifeCycle)
    {
        if (lifeCycle is IRefresh refresh)
        {
            refresh.Refresh();
        }

        return Task.CompletedTask;
    }

    public Task OnUnloaded(ILifeCycle lifeCycle)
    {
        return Task.CompletedTask;
    }
}