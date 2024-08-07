using Common.Lib.Service;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Common.Mvvm.Abstracts;

public abstract class BaseViewModel : ObservableObject, ILifeCycle, IPluginBuilder
{
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

    #region Builder

    public IPluginBuilder PluginBuilder => this;
    
    public IPluginBuilder AddPlugin<T>() where T : ILifePlugin
    {
        return AddPlugin((ILifePlugin)Activator.CreateInstance(typeof(T))!);
    }

    public IPluginBuilder AddPlugin(ILifePlugin plugin)
    {
        Plugins.Add(plugin);
        return this;
    }

    #endregion
}