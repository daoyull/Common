using Common.Lib.Service;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Common.Mvvm.Abstracts;

public abstract class BaseViewModel : ObservableObject, ILifeCycle, IPluginBuilder
{
    public HashSet<ILifePlugin> Plugins { get; } = new();

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