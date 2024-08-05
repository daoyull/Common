using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.VisualTree;
using Common.Avalonia.Controls;

namespace Common.Avalonia.Abstracts;

public abstract partial class UserGridComponent<T, TS>
{
    protected abstract SelectedMode SelectedMode { get; }

    protected abstract DataGrid? DataGrid { get; }

    private MultiSelectButton? _multiSelectButton;

    /// <summary>
    /// 增加多选列
    /// </summary>
    private void RegisterMultiSelect()
    {
        if (ViewModel == null || SelectedMode == SelectedMode.None)
        {
            return;
        }

        // 增加控件
        var dataGrid = DataGrid;
        if (dataGrid == null)
        {
            return;
        }

        var selectColum = new DataGridTemplateColumn();
        _multiSelectButton = new MultiSelectButton();
        selectColum.HeaderTemplate = new FuncDataTemplate<TS>((_, _) =>
        {
            // 只有允许多选才可见
            _multiSelectButton.IsVisible = SelectedMode == SelectedMode.Multi;
            _multiSelectButton.Click += MultiSelectButtonClick;
            return _multiSelectButton;
        });
        selectColum.CanUserReorder = false;
        selectColum.CanUserSort = false;
        selectColum.CanUserResize = false;
        selectColum.IsReadOnly = true;


        var dataTemplate = new FuncDataTemplate<TS>((value, _) =>
        {
            var toggleRadioButton = new ToggleRadioButton();
            if (SelectedMode == SelectedMode.Single)
            {
                toggleRadioButton.GroupName = "DataGridToggleRadioButton";
            }

            toggleRadioButton.HorizontalAlignment = HorizontalAlignment.Center;
            toggleRadioButton.VerticalAlignment = VerticalAlignment.Center;
            toggleRadioButton.Classes.Add("DataGridRowSelect");
            toggleRadioButton.Click += (sender, _) =>
            {
                if (SelectedMode == SelectedMode.Single)
                {
                    ViewModel.DataSource.ResetSelected(false);
                }

                var box = sender as ToggleRadioButton;
                var index = ViewModel.DataSource.IndexOf(value);
                if (index != -1)
                {
                    ViewModel.DataSource.SetSelected(index, box!.IsChecked == true);
                }

                RefreshMultiSelect();
            };
            return toggleRadioButton;
        });

        selectColum.CellTemplate = dataTemplate;
        dataGrid.Columns.Insert(0, selectColum);
        ViewModel.OnSetDataSourceAction += OnSetDataSourceAction;
    }

    private void OnSetDataSourceAction()
    {
        RefreshMultiSelect(true);
    }

    private void MultiSelectButtonClick(object? sender, RoutedEventArgs e)
    {
        if (sender is not MultiSelectButton button || ViewModel == null)
        {
            return;
        }

        var status = button.SelectStatus;

        switch (status)
        {
            case SelectStatus.NotSelect:
                ViewModel.DataSource.ResetSelected(false);
                break;
            case SelectStatus.AllSelect:
                ViewModel.DataSource.ResetSelected(true);
                break;
        }

        RefreshMultiSelect();
    }


    private void UnRegisterMultiSelect()
    {
        DataGrid?.Columns.Clear();
        if (ViewModel != null)
        {
            ViewModel.OnSetDataSourceAction -= OnSetDataSourceAction;
            ViewModel.DataSource.Clear();
        }
    }


    private void RefreshMultiSelect(bool isReload = false)
    {
        if (ViewModel == null)
        {
            return;
        }

        var selectedList = ViewModel.DataSource.SelectedList;

        #region 更新Header状态

        var selectCount = selectedList.Count(it => it);
        if (selectCount <= 0)
        {
            _multiSelectButton?.ChangeSelectStatus(SelectStatus.NotSelect);
        }
        else if (selectCount < ViewModel.DataSource.Count)
        {
            _multiSelectButton?.ChangeSelectStatus(SelectStatus.PartSelect);
        }
        else
        {
            _multiSelectButton?.ChangeSelectStatus(SelectStatus.AllSelect);
        }

        #endregion

        #region 更新选中列显示

        if (DataGrid != null && !isReload)
        {
            var visuals = DataGrid.GetVisualDescendants()
                .Where(it => it.GetType() == typeof(ToggleRadioButton))
                .Cast<ToggleRadioButton>()
                .ToList();
            foreach (var toggleRadioButton in visuals)
            {
                var index = ViewModel.DataSource.IndexOf((TS)toggleRadioButton.DataContext!);
                if (index != -1)
                {
                    toggleRadioButton.IsChecked = selectedList[index];
                }
            }
        }

        #endregion
    }
}