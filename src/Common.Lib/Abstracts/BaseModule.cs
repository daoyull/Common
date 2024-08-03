using Autofac;

namespace Common.Lib.Abstracts;

public abstract class BaseModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        LoadService(builder);
    }

    /// <summary>
    /// Public 是注册子模块用
    /// </summary>
    public abstract void LoadService(ContainerBuilder builder);
}