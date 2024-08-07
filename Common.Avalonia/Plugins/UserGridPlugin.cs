using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.VisualTree;
using Common.Avalonia.Abstracts;
using Common.Avalonia.Controls;
using Common.Lib.Exceptions;
using Common.Lib.Service;
using Common.Mvvm.Abstracts;

namespace Common.Avalonia.Plugins;

public class UserGridPlugin<T, TS> : ILifePlugin
    where T : BaseTableViewModel<TS> where TS : class
{
    private MultiSelectButton? _multiSelectButton;

    public Task OnCreated(ILifeCycle lifeCycle)
    {
        return Task.CompletedTask;
    }


    public Task OnLoaded(ILifeCycle lifeCycle)
    {
        if (lifeCycle is not UserGridComponent<T, TS> userGridComponent)
        {
            throw new BusinessException("lifeCycle is not UserGridComponent");
        }

        RegisterMultiSelect(userGridComponent);
        return Task.CompletedTask;
    }

    public Task OnUnloaded(ILifeCycle lifeCycle)
    {
        if (lifeCycle is not UserGridComponent<T, TS> userGridComponent)
        {
            throw new BusinessException("lifeCycle is not UserGridComponent");
        }

        UnRegisterMultiSelect(userGridComponent);
        return Task.CompletedTask;
    }

    /// <summary>
    /// 增加多选列
    /// </summary>
    /// <param name="component"></param>
    private void RegisterMultiSelect(UserGridComponent<T, TS> component)
    {
        if (component.ViewModel == null || component.SelectedMode == SelectedMode.None)
        {
            return;
        }

        // 增加控件
        var dataGrid = component.DataGrid;
        if (dataGrid == null)
        {
            return;
        }

        var selectColum = new DataGridTemplateColumn();
        _multiSelectButton = new MultiSelectButton();
        selectColum.HeaderTemplate = new FuncDataTemplate<TS>((_, _) =>
        {
            // 只有允许多选才可见
            _multiSelectButton.IsVisible = component.SelectedMode == SelectedMode.Multi;
            _multiSelectButton.Click += (sender, e) =>
            {
                var status = _multiSelectButton.SelectStatus;

                switch (status)
                {
                    case SelectStatus.NotSelect:
                        component.ViewModel.DataSource.ResetSelected(false);
                        break;
                    case SelectStatus.AllSelect:
                        component.ViewModel.DataSource.ResetSelected(true);
                        break;
                }

                RefreshMultiSelect(component);
            };
            return _multiSelectButton;
        });
        selectColum.CanUserReorder = false;
        selectColum.CanUserSort = false;
        selectColum.CanUserResize = false;
        selectColum.IsReadOnly = true;


        var dataTemplate = new FuncDataTemplate<TS>((value, _) =>
        {
            var toggleRadioButton = new ToggleRadioButton();
            if (component.SelectedMode == SelectedMode.Single)
            {
                toggleRadioButton.GroupName = "DataGridToggleRadioButton";
            }

            toggleRadioButton.HorizontalAlignment = HorizontalAlignment.Center;
            toggleRadioButton.VerticalAlignment = VerticalAlignment.Center;
            toggleRadioButton.Classes.Add("DataGridRowSelect");
            toggleRadioButton.Click += (sender, _) =>
            {
                if (component.SelectedMode == SelectedMode.Single)
                {
                    component.ViewModel.DataSource.ResetSelected(false);
                }

                var box = sender as ToggleRadioButton;
                var index = component.ViewModel.DataSource.IndexOf(value);
                if (index != -1)
                {
                    component.ViewModel.DataSource.SetSelected(index, box!.IsChecked == true);
                }

                RefreshMultiSelect(component);
            };
            return toggleRadioButton;
        });

        selectColum.CellTemplate = dataTemplate;
        dataGrid.Columns.Insert(0, selectColum);
        component.ViewModel.OnSetDataSourceAction += () => OnSetDataSourceAction(component);
    }

    private void OnSetDataSourceAction(UserGridComponent<T, TS> component)
    {
        RefreshMultiSelect(component, true);
    }


    private void UnRegisterMultiSelect(UserGridComponent<T, TS> component)
    {
        component.DataGrid?.Columns.Clear();
        if (component.ViewModel != null)
        {
            component.ViewModel.OnSetDataSourceAction -= () => OnSetDataSourceAction(component);
            component.ViewModel.DataSource.Clear();
        }
    }


    private void RefreshMultiSelect(UserGridComponent<T, TS> component, bool isReload = false)
    {
        if (component.ViewModel == null)
        {
            return;
        }

        var selectedList = component.ViewModel.DataSource.SelectedList;

        #region 更新Header状态

        var selectCount = selectedList.Count(it => it);
        if (selectCount <= 0)
        {
            _multiSelectButton?.ChangeSelectStatus(SelectStatus.NotSelect);
        }
        else if (selectCount < component.ViewModel.DataSource.Count)
        {
            _multiSelectButton?.ChangeSelectStatus(SelectStatus.PartSelect);
        }
        else
        {
            _multiSelectButton?.ChangeSelectStatus(SelectStatus.AllSelect);
        }

        #endregion

        #region 更新选中列显示

        if (component.DataGrid != null && !isReload)
        {
            var visuals = component.DataGrid.GetVisualDescendants()
                .Where(it => it.GetType() == typeof(ToggleRadioButton))
                .Cast<ToggleRadioButton>()
                .ToList();
            foreach (var toggleRadioButton in visuals)
            {
                var index = component.ViewModel.DataSource.IndexOf((TS)toggleRadioButton.DataContext!);
                if (index != -1)
                {
                    toggleRadioButton.IsChecked = selectedList[index];
                }
            }
        }

        #endregion
    }
}