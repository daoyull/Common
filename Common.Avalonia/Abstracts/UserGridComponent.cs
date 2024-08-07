using Avalonia.Controls;
using Avalonia.Interactivity;
using Common.Avalonia.Plugins;
using Common.Mvvm.Abstracts;

namespace Common.Avalonia.Abstracts;

/// <summary>
/// 一个表格用一个该用户控件
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TS"></typeparam>
public abstract partial class UserGridComponent<T, TS> : UserComponent<T>
    where T : BaseTableViewModel<TS> where TS : class
{
    public UserGridComponent()
    {
        Plugins.Add(new UserGridPlugin<T, TS>());
    }

    public abstract SelectedMode SelectedMode { get; }

    public abstract DataGrid? DataGrid { get; }
}

public enum SelectedMode
{
    None = 0,
    Single = 1,
    Multi = 2
}