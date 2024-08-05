using System.Collections.ObjectModel;
using Common.Mvvm.Models;

namespace Common.Mvvm.Abstracts;

public abstract class BaseTableViewModel<T> : BaseViewModel where T : class
{
    public BaseTableViewModel()
    {
    }
    

    #region 属性

    private MvvmPageQuery? _query;

    /// <summary>
    /// 查询参数
    /// </summary>
    public MvvmPageQuery? Query
    {
        get => _query;
        set => SetProperty(ref _query, value);
    }

    private ObservableCollection<T> _dataSource = new();


    /// <summary>
    /// 数据源
    /// </summary>
    public ObservableCollection<T> DataSource => _dataSource;

    private ObservableCollection<T> _selectedRows = new();

    /// <summary>
    /// 选中的行
    /// </summary>
    public ObservableCollection<T> SelectedRows => _selectedRows;

    #endregion

    #region Command

    #endregion

    #region 重写

    public override async Task OnLoaded()
    {
        await base.OnLoaded();
        await RefreshUi();
    }

    #endregion


    #region Method

    /// <summary>
    /// 设置数据源
    /// </summary>
    /// <param name="source"></param>
    public void SetDataSource(IEnumerable<T> source)
    {
        DataSource.Clear();
        foreach (var t in source)
        {
            DataSource.Add(t);
        }
    }

    #endregion
}