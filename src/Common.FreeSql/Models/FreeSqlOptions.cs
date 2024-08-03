using FreeSql;
using Microsoft.Extensions.Logging;

namespace Common.FreeSql.Models;

public class FreeSqlOptions
{
    public DataType DataType { get; set; }

    public string? ConnectionString { get; set; }

    public bool EnableNoneCommandParameter { get; set; }

    public bool EnableAutoSyncStructure { get; set; }

    public string[]? SlaveConnectionStrings { get; set; }

    public bool EnableJsonMap { get; set; }

    public bool EnableLogger { get; set; }

    public LogLevel? LogLevel { get; set; }
}