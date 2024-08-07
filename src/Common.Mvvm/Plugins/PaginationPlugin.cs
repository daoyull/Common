using Common.Lib.Service;
using Common.Mvvm.Models;
using Common.Mvvm.Service;

namespace Common.Mvvm.Plugins;

public class PaginationPlugin : ILifePlugin
{
    public Task OnCreated(ILifeCycle lifeCycle)
    {
        return Task.CompletedTask;
    }

    public Task OnLoaded(ILifeCycle lifeCycle)
    {
        CallPaginationMethod(lifeCycle.GetType(), nameof(IPagination<MvvmPageQuery>.BeginListener));
        return Task.CompletedTask;
    }

    public Task OnUnloaded(ILifeCycle lifeCycle)
    {
        CallPaginationMethod(lifeCycle.GetType(), nameof(IPagination<MvvmPageQuery>.EndListener));
        return Task.CompletedTask;
    }

    private void CallPaginationMethod(Type type, string methodName)
    {
        var paginationInterface = type
            .GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPagination<>));

        if (paginationInterface != null)
        {
            var beginListenerMethod = paginationInterface.GetMethod(methodName);
            if (beginListenerMethod != null)
            {
                beginListenerMethod.Invoke(this, null);
            }
        }
    }
}