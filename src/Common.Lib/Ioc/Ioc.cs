using Autofac;

namespace Common.Lib.Ioc;

public static partial class Ioc
{
    private static volatile IContainer? _container;

    /// <summary>
    /// Ioc容器
    /// </summary>
    public static IContainer? Container => _container;

    /// <summary>
    /// 是否已Builder
    /// </summary>
    public static bool IsBuilder => _container != null;

    public static object Resolve(Type type)
    {
        return IsBuilder ? Container!.Resolve(type) : throw new InvalidOperationException("Ioc not loaded");
    }

    public static object? ResolveOptional(Type type)
    {
        return IsBuilder ? Container!.ResolveOptional(type) : throw new InvalidOperationException("Ioc not loaded");
    }

    public static T Resolve<T>() where T : notnull
    {
        return IsBuilder ? Container!.Resolve<T>() : throw new InvalidOperationException("Ioc not loaded");
    }

    public static T? ResolveOptional<T>() where T : class
    {
        return IsBuilder ? Container!.ResolveOptional<T>() : throw new InvalidOperationException("Ioc not loaded");
    }

    public static void Dispose()
    {
        Container?.Dispose();
    }
}