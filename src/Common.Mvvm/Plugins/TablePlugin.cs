using Common.Lib.Service;
using Common.Mvvm.Abstracts;

namespace Common.Mvvm.Plugins;

public class TablePlugin<T> : ILifePlugin where T : class
{
    public Task OnCreate(ILifeCycle lifeCycle)
    {
        return Task.CompletedTask;
    }

    public Task OnInit(ILifeCycle lifeCycle)
    {
        return Task.CompletedTask;
    }

    public Task OnLoad(ILifeCycle lifeCycle)
    {
        if (lifeCycle is not BaseTableViewModel<T> tableViewModel)
        {
            return Task.CompletedTask;
        }

        tableViewModel.DataSource.SelectedChanged += () => { HandleSelectedChanged(tableViewModel); };
        return Task.CompletedTask;
    }

    public Task OnUnload(ILifeCycle lifeCycle)
    {
        if (lifeCycle is not BaseTableViewModel<T> tableViewModel)
        {
            return Task.CompletedTask;
        }

        tableViewModel.DataSource.SelectedChanged -= () => { HandleSelectedChanged(tableViewModel); };
        return Task.CompletedTask;
    }

    private void HandleSelectedChanged(BaseTableViewModel<T> tableViewModel)
    {
        var list = new List<T>();
        for (var i = 0; i < tableViewModel.DataSource.Count; i++)
        {
            if (tableViewModel.DataSource.SelectedList[i])
            {
                list.Add(tableViewModel.DataSource[i]);
            }
        }

        tableViewModel.SelectedRows = list;

        tableViewModel.IsSelectedSingle = tableViewModel.SelectedRows.Count == 1;
        tableViewModel.SelectedRow = tableViewModel.IsSelectedSingle ? tableViewModel.SelectedRows[0] : null;
    }
}