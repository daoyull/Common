using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;

namespace Common.Mvvm.Models;

public partial class BasePageQuery : ObservableObject
{
    /// <summary>
    /// 页数
    /// </summary>
    [JsonProperty("pageNum")] [ObservableProperty]
    private int _pageNum = 1;

    /// <summary>
    /// 页码
    /// </summary>
    [JsonProperty("pageSize")] [ObservableProperty]
    private int _pageSize = 10;
}