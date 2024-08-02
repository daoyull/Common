using Common.Lib.Helpers;
using Common.Lib.Service;

namespace Common.Lib.Attributes;
[AttributeUsage(AttributeTargets.Property)]
public class GuidAttribute: Attribute, IIdGenerator<string>
{
    public string Generate()
    {
        return IdHelper.Guid;
    }
}