namespace Common.Lib.Service;

public interface IPluginBuilder
{
    IPluginBuilder AddPlugin<T>() where T : ILifePlugin;
    IPluginBuilder AddPlugin(ILifePlugin plugin);
}