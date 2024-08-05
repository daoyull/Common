using Common.Mvvm.Models;
using Common.Mvvm.Service;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Common.Mvvm.Abstracts;

public abstract class BaseViewModel : ObservableObject
{
    /// <summary>
    /// 获取数据刷新UI
    /// </summary>
    public abstract Task Refresh();

    public virtual async Task OnLoaded()
    {
        CallPaginationMethod(nameof(IPagination<MvvmPageQuery>.BeginListener));
        await Refresh();
    }

    public virtual Task OnUnloaded()
    {
        CallPaginationMethod(nameof(IPagination<MvvmPageQuery>.EndListener));
        return Task.CompletedTask;
    }

    private void CallPaginationMethod(string methodName)
    {
        var type = GetType();
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