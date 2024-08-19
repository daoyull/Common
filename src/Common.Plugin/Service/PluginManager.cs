using Autofac;
using Common.FreeSql;
using Common.Lib.Ioc;

namespace Common.Plugin.Service;

public static class PluginManager
{
    public static void LoadPlugin(IPlugin plugin,ContainerBuilder containerBuilder)
    {
        var resolver = Ioc.Resolve<FreeSqlResolver>();
        var freeSql = resolver("plugin");
        
        // todo 校验数据库信息和dll中的信息
        plugin.ServiceLoad(containerBuilder);
        
        
    }
}