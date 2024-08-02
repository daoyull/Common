using Autofac;

namespace Common.Lib.Service;

public interface IModule : IDisposable
{
    /// <summary>
    /// 容器
    /// </summary>
    ILifetimeScope LifetimeScope { get; }

    void OnLoading();

    /// <summary>
    /// 加载模块
    /// </summary>
    /// <param name="builder"></param>
    public void Load(ContainerBuilder builder);

    void OnLoaded();
}