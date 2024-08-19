namespace Common.Plugin.Models;

public class PluginConfig
{
    public string PluginId { get; set; } = null!;

    /// <summary>
    /// 插件名称
    /// </summary>
    public string? Name { get; set; }

    public List<RelPluginFile>? Dlls { get; set; }
}