using Autofac;

namespace Common.Plugin.Service;

public interface IPlugin
{
    public IPluginInfo Info { get; }
    
    public void ServiceLoad(ContainerBuilder builder);
    
    
}