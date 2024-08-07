using Common.Lib.Service;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Common.Mvvm.Abstracts;

public abstract class BaseViewModel : ObservableObject, ILifeCycle, IRefresh
{
    /// <summary>
    /// 获取数据刷新UI
    /// </summary>
    public virtual Task Refresh()
    {
        return Task.CompletedTask;
    }

    #region 生命周期

    public HashSet<ILifePlugin> Plugins { get; } = new();

    public virtual async Task OnCreated()
    {
        foreach (var lifePlugin in Plugins)
        {
            await lifePlugin.OnCreated(this);
        }
    }

    public virtual async Task OnLoaded()
    {
        foreach (var lifePlugin in Plugins)
        {
            await lifePlugin.OnLoaded(this);
        }
    }

    public virtual async Task OnUnloaded()
    {
        foreach (var lifePlugin in Plugins)
        {
            await lifePlugin.OnUnloaded(this);
        }

        Plugins.Clear();
    }

    #endregion
}