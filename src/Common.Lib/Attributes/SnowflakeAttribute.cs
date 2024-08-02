using Common.Lib.Helpers;
using Common.Lib.Service;
using Yitter.IdGenerator;

namespace Common.Lib.Attributes;

/// <summary>
/// 雪花ID
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class SnowflakeAttribute : Attribute, IIdGenerator<long>
{
    public long Generate()
    {
        return IdHelper.SnowId;
    }
}