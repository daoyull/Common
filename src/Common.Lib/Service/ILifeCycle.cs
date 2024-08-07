namespace Common.Lib.Service;

public interface ILifeCycle
{
    public HashSet<ILifePlugin> Plugins { get; }
    
    /// <summary>
    /// 创建
    /// </summary>
    Task OnCreated();
    
    /// <summary>
    /// 加载
    /// </summary>
    /// <returns></returns>
    Task OnLoaded();
    

    /// <summary>
    /// 卸载
    /// </summary>
    /// <returns></returns>
    Task OnUnloaded();
}