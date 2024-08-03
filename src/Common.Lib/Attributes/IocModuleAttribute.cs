namespace Common.Lib.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class IocModuleAttribute : Attribute
{
    public IocModuleAttribute(Type moduleType)
    {
        ModuleType = moduleType;
    }

    public Type ModuleType { get; set; }
}