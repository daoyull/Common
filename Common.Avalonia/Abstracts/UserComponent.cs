using System.Reflection;
using Autofac;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Common.Lib.Attributes;
using Common.Lib.Ioc;
using Common.Mvvm.Abstracts;
using Microsoft.Extensions.Logging;

namespace Common.Avalonia.Abstracts;

/// <summary>
/// 用户组件
/// </summary>
public abstract class UserComponent<T> : UserControl where T : BaseViewModel
{
    /// <summary>
    /// Autofac 获取ViewModel
    /// </summary>
    public T? ViewModel { get; set; }

    protected ILogger<UserComponent<T>>? Logger;

    public UserComponent()
    {
        // Ioc加载ViewModel
        // 设计期加载异常
        try
        {
           
            ViewModel = Ioc.IsBuilder ? Ioc.Container?.ResolveOptional<T>() : default;
            Logger = Ioc.Resolve<ILogger<UserComponent<T>>>();
            DataContext = ViewModel;
        }
        catch (Exception e)
        {
            // ignored
        }

        // var moduleAttribute = GetType().GetCustomAttribute<IocModuleAttribute>();
        // DataContext = ViewModel =
        //     moduleAttribute == null
        //         ?
        //         // 主容器
        //         Ioc.Container?.ResolveOptional<T>()
        //         // 子模块
        //         : Ioc.GetSubModuleIoc(moduleAttribute.ModuleType)?.ResolveOptional<T>();
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        OnLoaded();
        ViewModel?.OnLoaded();
    }

    protected virtual void OnLoaded()
    {
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        base.OnUnloaded(e);
        OnUnloaded();
        ViewModel?.OnUnloaded();
    }

    protected virtual void OnUnloaded()
    {
    }
}