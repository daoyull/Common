using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

namespace Common.Mvvm.Abstracts;

public abstract class BaseTableViewModel<T> : BaseViewModel where T : class
{
    public override Task OnLoaded()
    {
        DataSource.SelectedChanged += HandleSelectedChanged;
        return base.OnLoaded();
    }


    private void HandleSelectedChanged()
    {
        var list = new List<T>();
        for (var i = 0; i < DataSource.Count; i++)
        {
            if (DataSource.SelectedList[i])
            {
                list.Add(DataSource[i]);
            }
        }

        SelectedRows = list;

        IsSelectedSingle = SelectedRows.Count == 1;
        SelectedRow = IsSelectedSingle ? SelectedRows[0] : null;
    }

    public override Task OnUnloaded()
    {
        DataSource.SelectedChanged -= HandleSelectedChanged;
        return base.OnUnloaded();
    }

    #region Command

    /// <summary>
    /// 刷新 搜索
    /// </summary>
    public AsyncRelayCommand? RefreshCommand { get; protected set; }

    /// <summary>
    /// 重置搜索栏
    /// </summary>
    public AsyncRelayCommand? ResetSearchBarCommand { get; protected set; }

    /// <summary>
    /// 新增行
    /// </summary>
    public AsyncRelayCommand? AddRowCommand { get; protected set; }

    /// <summary>
    /// 修改行
    /// </summary>
    public AsyncRelayCommand<T>? EditRowCommand { get; protected set; }

    /// <summary>
    /// 多行删除
    /// </summary>
    public AsyncRelayCommand<List<T>>? DeleteRowsCommand { get; protected set; }

    /// <summary>
    /// 单行删除
    /// </summary>
    public AsyncRelayCommand<T>? DeleteRowCommand { get; protected set; }

    #endregion

    #region 属性

    /// <summary>
    /// 数据源
    /// </summary>
    public MvvmSelectObservableCollection<T> DataSource { get; } = new();

    private bool _isSelectedSingle;

    /// <summary>
    /// 是否只选中了一行
    /// </summary>
    public bool IsSelectedSingle
    {
        get => _isSelectedSingle;
        set => SetProperty(ref _isSelectedSingle, value);
    }

    private T? _selectedRow;

    public T? SelectedRow
    {
        get => _selectedRow;
        set => SetProperty(ref _selectedRow, value);
    }


    private List<T> _selectRows = new();

    public List<T> SelectedRows
    {
        get => _selectRows;
        private set => SetProperty(ref _selectRows, value);
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

        OnSetDataSourceAction?.Invoke();
    }

    #endregion

    #region 委托

    public Action? OnSetDataSourceAction { get; set; }

    #endregion
}

public class MvvmSelectObservableCollection<T> : ObservableCollection<T> where T : notnull
{
    public Action? SelectedChanged { get; set; }
    public List<bool> SelectedList { get; } = new();

    public bool IsSelected(int index)
    {
        return SelectedList[index];
    }

    public void ResetSelected(bool isSelected)
    {
        SelectedList.Clear();
        for (int i = 0; i < Count; i++)
        {
            SelectedList.Add(isSelected);
        }

        SelectedChanged?.Invoke();
    }

    public void SetSelected(int index, bool isSelected)
    {
        SelectedList[index] = isSelected;
        SelectedChanged?.Invoke();
    }


    protected override void InsertItem(int index, T item)
    {
        SelectedList.Insert(index, false);
        base.InsertItem(index, item);
        SelectedChanged?.Invoke();
    }

    protected override void MoveItem(int oldIndex, int newIndex)
    {
        SelectedList.RemoveAt(oldIndex);
        SelectedList.Insert(newIndex, false);
        base.MoveItem(oldIndex, newIndex);
        SelectedChanged?.Invoke();
    }

    protected override void SetItem(int index, T item)
    {
        SelectedList[index] = false;
        base.SetItem(index, item);
        SelectedChanged?.Invoke();
    }

    protected override void ClearItems()
    {
        SelectedList.Clear();
        base.ClearItems();
        SelectedChanged?.Invoke();
    }

    protected override void RemoveItem(int index)
    {
        SelectedList.RemoveAt(index);
        base.RemoveItem(index);
        SelectedChanged?.Invoke();
    }
}