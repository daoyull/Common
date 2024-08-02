using Autofac;
using Common.Lib.Service;

namespace Common.Lib.Ioc;

public static class Extensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="lifetime"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns>SubModule LifetimeScope</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static ILifetimeScope RegisterSubModule<T>(this ILifetimeScope lifetime) where T : IModule
    {
        var instance = Activator.CreateInstance(typeof(T));
        if (instance is not IModule module)
        {
            throw new InvalidOperationException();
        }

        return RegisterSubModule(lifetime, module);
    }

    public static ILifetimeScope RegisterSubModule(this ILifetimeScope lifetime, IModule module)
    {
        module.OnLoading();
        var subModuleLifetimeScope = lifetime.BeginLifetimeScope(module.Load);
        module.OnLoaded();
        return subModuleLifetimeScope;
    }
}