/*using System.Collections.Concurrent;
using Autofac;
using Common.Lib.Abstracts;

namespace Common.Lib.Ioc;

/// <summary>
/// https://learn.microsoft.com/zh-cn/dotnet/standard/assembly/unloadability
/// https://github.com/dotnet/samples/tree/main/core/tutorials/Unloading
/// 插件热插拔 Builder后注册子模块
/// 问题1 Autofac未能回收
/// 问题2 Avalinia和WPF创建的控件未能Unload
/// </summary>
public static partial class Ioc
{
    /// <summary>
    /// todo 树形支持
    /// 当前是1级结构，子模块尽量不在加载子模块
    /// </summary>
    private static readonly ConcurrentDictionary<Type, ILifetimeScope> SubModuleLifetimeScopes = new();

    public static ILifetimeScope? GetSubModuleIoc(Type type)
    {
        return SubModuleLifetimeScopes.GetValueOrDefault(type);
    }

    /// <summary>
    /// 需要在Builder之后注册子模块
    /// https://autofac-.readthedocs.io/en/latest/lifetime/working-with-scopes.html
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static ILifetimeScope RegisterSubModule<T>(ILifetimeScope container) where T : BaseModule
    {
        var modelType = typeof(T);
        if (SubModuleLifetimeScopes.ContainsKey(modelType))
        {
            return container;
        }

        BaseModule? instance = (BaseModule?)Activator.CreateInstance(modelType);
        if (instance == null)
        {
            throw new Exception("Create Instance Error");
        }

        var beginLifetimeScope = container.BeginLifetimeScope(builder => { instance.LoadService(builder); });

        SubModuleLifetimeScopes[modelType] = beginLifetimeScope;
        return container;
    }


    public static ILifetimeScope UnRegisterSubModule<T>(ILifetimeScope container) where T : BaseModule
    {
        var modelType = typeof(T);
        if (!SubModuleLifetimeScopes.TryRemove(modelType, out var lifetimeScope))
        {
            return container;
        }

        lifetimeScope.Dispose();
        return container;
    }
}*/