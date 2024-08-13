using Common.Lib.Service;
using Common.Mvvm.Models;
using Common.Mvvm.Service;

namespace Common.Mvvm.Plugins;

public class PaginationPlugin : ILifePlugin
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
        CallPaginationMethod(lifeCycle, nameof(IPagination<MvvmPageQuery>.BeginListener));
        return Task.CompletedTask;
    }

    public Task OnUnload(ILifeCycle lifeCycle)
    {
        CallPaginationMethod(lifeCycle, nameof(IPagination<MvvmPageQuery>.EndListener));
        return Task.CompletedTask;
    }

    private void CallPaginationMethod(ILifeCycle lifeCycle, string methodName)
    {
        var paginationInterface = lifeCycle.GetType()
            .GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPagination<>));

        if (paginationInterface != null)
        {
            var beginListenerMethod = paginationInterface.GetMethod(methodName);
            if (beginListenerMethod != null)
            {
                beginListenerMethod.Invoke(lifeCycle, null);
            }
        }
    }
}