using Common.Mvvm.Models;
using Common.Mvvm.Service;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Common.Mvvm.Abstracts;

public abstract class BaseViewModel : ObservableObject
{
    public BaseViewModel()
    {
    }


    /// <summary>
    /// 获取数据刷新UI
    /// </summary>
    public virtual Task Refresh()
    {
        return Task.CompletedTask;
    }


    public virtual Task OnLoaded()
    {
        CallPaginationMethod(nameof(IPagination<MvvmPageQuery>.BeginListener));
        return Task.CompletedTask;
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