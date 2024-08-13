using Avalonia.Controls;
using Avalonia.Interactivity;
using Common.Avalonia.Plugins;
using Common.Lib.Service;
using Common.Mvvm.Abstracts;

namespace Common.Avalonia.Abstracts;

public abstract class UserComponent : UserControl, ILifeCycle, IPluginBuilder
{
    public HashSet<ILifePlugin> Plugins { get; } = new();

    public override async void EndInit()
    {
        base.EndInit();
        await ((ILifeCycle)this).OnCreate();
    }

    protected override async void OnInitialized()
    {
        base.OnInitialized();
        await ((ILifeCycle)this).OnInit();
    }

    protected override async void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        await ((ILifeCycle)this).OnLoad();
    }

    protected override async void OnUnloaded(RoutedEventArgs e)
    {
        base.OnUnloaded(e);
        await ((ILifeCycle)this).OnUnload();
    }


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