using Avalonia.Controls;
using Avalonia.Interactivity;
using Common.Avalonia.Plugins;
using Common.Lib.Service;
using Common.Mvvm.Abstracts;

namespace Common.Avalonia.Abstracts;

public abstract class UserComponent : UserControl, ILifeCycle
{
    public HashSet<ILifePlugin> Plugins { get; } = new();

    protected override async void OnInitialized()
    {
        base.OnInitialized();
        await OnCreated();
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
        Plugins.Add(new ViewModelPlugin<T>());
    }
}