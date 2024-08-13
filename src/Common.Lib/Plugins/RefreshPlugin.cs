using Common.Lib.Service;

namespace Common.Lib.Plugins;

public class RefreshPlugin : ILifePlugin
{
    public Task OnCreate(ILifeCycle lifeCycle)
    {
        return Task.CompletedTask;
    }

    public Task OnInit(ILifeCycle lifeCycle)
    {
        return Task.CompletedTask;
    }

    public Task OnLoad(ILifeCycle lifeCycle)
    {
        if (lifeCycle is IRefresh refresh)
        {
            refresh.Refresh();
        }

        return Task.CompletedTask;
    }

    public Task OnUnload(ILifeCycle lifeCycle)
    {
        return Task.CompletedTask;
    }
}