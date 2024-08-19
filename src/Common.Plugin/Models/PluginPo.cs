using FreeSql.DataAnnotations;

namespace Common.Plugin.Models;

[Table(Name = "plugin")]
public class PluginPo
{
    /// <summary>
    /// 插件Id
    /// </summary>
    [Column(IsNullable = false)]
    public string PluginId { get; set; } = null!;

    /// <summary>
    /// 插件名称
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 说明
    /// </summary>
    public string? Desc { get; set; }

    /// <summary>
    /// 版本
    /// </summary>
    public string? Version { get; set; }

    /// <summary>
    /// Icon
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// 作者
    /// </summary>
    public string? Author { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool IsEnable { get; set; } = true;

    /// <summary>
    /// 是否默认加载
    /// </summary>
    public bool IsDefaultLoad { get; set; } = false;

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
}