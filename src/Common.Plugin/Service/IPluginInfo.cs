namespace Common.Plugin.Service;

public interface IPluginInfo
{
    public string PluginId { get; }

    public string Version { get; }

    public string PluginName { get; }

    public string Desc { get; set; }
}