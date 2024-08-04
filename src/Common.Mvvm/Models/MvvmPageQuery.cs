using Common.Lib.Service;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;

namespace Common.Mvvm.Models;

public partial class MvvmPageQuery : ObservableObject, IPageQuery
{
    /// <summary>
    /// 页数
    /// </summary>
    [property: JsonProperty("pageNum")] [ObservableProperty]
    private int _pageNum = 1;

    /// <summary>
    /// 页码
    /// </summary>
    [property: JsonProperty("pageSize")] [ObservableProperty]
    private int _pageSize = 10;

    [property: JsonIgnore] [ObservableProperty]
    private long _total;
}