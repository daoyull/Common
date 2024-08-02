using Yitter.IdGenerator;

namespace Common.Lib.Helpers;

public class IdHelper
{
    static IdHelper()
    {
        // 创建 IdGeneratorOptions 对象，可在构造函数中输入 WorkerId：
        var options = new IdGeneratorOptions();
        // options.WorkerIdBitLength = 10; // 默认值6，限定 WorkerId 最大值为2^6-1，即默认最多支持64个节点。
        // options.SeqBitLength = 6; // 默认值6，限制每毫秒生成的ID个数。若生成速度超过5万个/秒，建议加大 SeqBitLength 到 10。
        // options.BaseTime = Your_Base_Time; // 如果要兼容老系统的雪花算法，此处应设置为老系统的BaseTime。
        // ...... 其它参数参考 IdGeneratorOptions 定义。

        // 保存参数（务必调用，否则参数设置不生效）：
        YitIdHelper.SetIdGenerator(options);
    }

    /// <summary>
    /// Guid，不带 - 
    /// </summary>
    public static string SimpleGuid => System.Guid.NewGuid().ToString("N");

    public static string Guid => System.Guid.NewGuid().ToString();


    /// <summary>
    /// 生成雪花id
    /// </summary>
    public static long SnowId => YitIdHelper.NextId();
}