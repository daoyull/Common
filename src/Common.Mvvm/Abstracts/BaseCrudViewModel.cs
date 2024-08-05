using System.Collections.ObjectModel;
using Common.Mvvm.Models;

namespace Common.Mvvm.Abstracts;

public abstract class BaseTableViewModel<T> : BaseViewModel where T : class
{
    public BaseTableViewModel()
    {
    }


    #region 属性

    /// <summary>
    /// 数据源
    /// </summary>
    public MvvmSelectObservableCollection<T> DataSource { get; } = new();

    /// <summary>
    /// 选中的行
    /// </summary>
    public List<T> SelectedRows
    {
        get
        {
            var list = new List<T>();
            for (var i = 0; i < DataSource.Count; i++)
            {
                if (DataSource.SelectedList[i])
                {
                    list.Add(DataSource[i]);
                }
            }

            return list;
        }
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
    
    public Action? OnSetDataSourceAction { get; set; }

    #endregion
}

public class MvvmSelectObservableCollection<T> : ObservableCollection<T> where T : notnull
{
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
    }

    public void SetSelected(int index, bool isSelected)
    {
        SelectedList[index] = isSelected;
    }


    protected override void InsertItem(int index, T item)
    {
        SelectedList.Insert(index, false);
        base.InsertItem(index, item);
    }

    protected override void MoveItem(int oldIndex, int newIndex)
    {
        SelectedList.RemoveAt(oldIndex);
        SelectedList.Insert(newIndex, false);
        base.MoveItem(oldIndex, newIndex);
    }

    protected override void SetItem(int index, T item)
    {
        SelectedList[index] = false;
        base.SetItem(index, item);
    }

    protected override void ClearItems()
    {
        SelectedList.Clear();
        base.ClearItems();
    }

    protected override void RemoveItem(int index)
    {
        SelectedList.RemoveAt(index);
        base.RemoveItem(index);
    }
}