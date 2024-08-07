using Avalonia.Controls;
using Avalonia.Interactivity;
using Common.Avalonia.Plugins;
using Common.Lib.Service;
using Common.Mvvm.Abstracts;

namespace Common.Avalonia.Abstracts;

public abstract class UserComponent : UserControl, ILifeCycle, IPluginBuilder
{
    public override void EndInit()
    {
        base.EndInit();
        OnLoaded();
    }

    public HashSet<ILifePlugin> Plugins { get; } = new();

    public IPluginBuilder PluginBuilder => this;


    protected virtual void LoadPlugin(IPluginBuilder builder)
    {
    }

    protected override async void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        await OnLoaded();
    }

    protected override async void OnUnloaded(RoutedEventArgs e)
    {
        base.OnUnloaded(e);
        await OnUnloaded();
    }

    public Task OnCreated()
    {
        foreach (var lifePlugin in Plugins)
        {
            lifePlugin.OnCreated(this);
        }

        return Task.CompletedTask;
    }

    public Task OnLoaded()
    {
        foreach (var lifePlugin in Plugins)
        {
            lifePlugin.OnLoaded(this);
        }

        return Task.CompletedTask;
    }

    public Task OnUnloaded()
    {
        foreach (var lifePlugin in Plugins)
        {
            lifePlugin.OnUnloaded(this);
        }

        Plugins.Clear();
        return Task.CompletedTask;
    }

    public IPluginBuilder AddPlugin<T>() where T : ILifePlugin
    {
        return AddPlugin((ILifePlugin)Activator.CreateInstance(typeof(T))!);
    }

    public IPluginBuilder AddPlugin(ILifePlugin plugin)
    {
        Plugins.Add(plugin);
        return this;
    }
}

public interface IPluginBuilder
{
    IPluginBuilder AddPlugin<T>() where T : ILifePlugin;
    IPluginBuilder AddPlugin(ILifePlugin plugin);
}

/// <summary>
/// 用户组件
/// </summary>
public abstract class UserComponent<T> : UserComponent where T : BaseViewModel
{
    /// <summary>
    /// Autofac 获取ViewModel
    /// </summary>
    public T? ViewModel { get; set; }

    public UserComponent()
    {
        PluginBuilder.AddPlugin<ViewModelPlugin<T>>();
    }
}