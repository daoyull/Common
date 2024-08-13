namespace Common.Lib.Service;

public interface ILifeCycle
{
    public HashSet<ILifePlugin> Plugins { get; }

    /// <summary>
    /// 创建
    /// </summary>
    async Task OnCreate()
    {
        foreach (var lifePlugin in Plugins)
        {
            await lifePlugin.OnCreate(this);
        }
    }

    /// <summary>
    /// 初始化完成
    /// </summary>
    /// <returns></returns>
    async Task OnInit()
    {
        foreach (var lifePlugin in Plugins)
        {
            await lifePlugin.OnInit(this);
        }
    }

    /// <summary>
    /// 加载
    /// </summary>
    /// <returns></returns>
    async Task OnLoad()
    {
        foreach (var lifePlugin in Plugins)
        {
            await lifePlugin.OnLoad(this);
        }
    }


    /// <summary>
    /// 卸载
    /// </summary>
    /// <returns></returns>
    async Task OnUnload()
    {
        foreach (var lifePlugin in Plugins)
        {
            await lifePlugin.OnUnload(this);
        }
    }
}