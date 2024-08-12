using Autofac;

namespace Common.Lib.Ioc;

public static partial class Ioc
{
    private static Action<ContainerBuilder>? _builderAction;

    /// <summary>
    /// 注册服务
    /// </summary>
    /// <param name="builderAction">注册服务委托</param>
    public static void Register(Action<ContainerBuilder> builderAction)
    {
        _builderAction += builderAction;
    }

    /// <summary>
    /// 构造容器
    /// </summary>
    public static void Builder()
    {
        var containerBuilder = new ContainerBuilder();
        _builderAction?.Invoke(containerBuilder);
        _container = containerBuilder.Build();
    }

    public static void SetRootContainer(ILifetimeScope lifetimeScope)
    {
        _container = lifetimeScope;
    }
}