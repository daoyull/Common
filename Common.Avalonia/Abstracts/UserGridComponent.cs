using Avalonia.Interactivity;
using Common.Mvvm.Abstracts;

namespace Common.Avalonia.Abstracts;

public abstract partial class UserGridComponent<T, TS> : UserComponent<T>
    where T : BaseTableViewModel<TS> where TS : class
{
    protected override void OnLoaded(RoutedEventArgs e)
    {
        RegisterMultiSelect();
        base.OnLoaded(e);
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        UnRegisterMultiSelect();
        base.OnLoaded(e);
    }
}

public enum SelectedMode
{
    None = 0,
    Single,
    Multi
}