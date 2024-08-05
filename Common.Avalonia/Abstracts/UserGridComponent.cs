using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.VisualTree;
using Common.Avalonia.Controls;
using Common.Lib.Helpers;
using Common.Mvvm.Abstracts;
using CommunityToolkit.Mvvm.Input;

namespace Common.Avalonia.Abstracts;

public abstract class UserGridComponent<T, TS> : UserComponent<T> where T : BaseTableViewModel<TS> where TS : class
{
  
    protected abstract SelectedMode SelectedMode { get; }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        AddMultiColum();
        base.OnLoaded(e);
    }


    MultiSelectButton? multiSelectButton ;
    /// <summary>
    /// 增加多选列
    /// </summary>
    private void AddMultiColum()
    {
        if (ViewModel == null || SelectedMode == SelectedMode.None)
        {
            return;
        }

        // 增加控件
        var dataGrid = this.FindControl<DataGrid>("MainDataGrid");
        if (dataGrid == null)
        {
            return;
        }

        var selectColum = new DataGridTemplateColumn();
        selectColum.CanUserReorder = false;
        selectColum.CanUserSort = false;
        selectColum.CanUserResize = false;
        selectColum.IsReadOnly = true;
        multiSelectButton = new();
        // 只有允许多选才可见
        multiSelectButton.IsVisible = SelectedMode == SelectedMode.Multi;
        multiSelectButton.Click += MultiSelectButtonClick;
        
        selectColum.Header = multiSelectButton;

        var dataTemplate = new FuncDataTemplate<TS>((value, nameScope) =>
        {
            var checkBox = new ToggleRadioButton();
            checkBox.Name = "SelectRadioButton";
            if (SelectedMode == SelectedMode.Single)
            {
                checkBox.GroupName = "MainDataGridRadioButton";
            }

            checkBox.HorizontalAlignment = HorizontalAlignment.Center;
            checkBox.VerticalAlignment = VerticalAlignment.Center;
            checkBox.Classes.Add("DataGridRowSelect");
            checkBox.IsCheckedChanged += (sender, args) =>
            {
                var box = sender as RadioButton;
                if (SelectedMode == SelectedMode.Single)
                {
                    ViewModel.SelectedRows.Clear();
                }

                if (box!.IsChecked == true)
                {
                    ViewModel.SelectedRows.Add(value);
                }
                else
                {
                    ViewModel.SelectedRows.Remove(value);
                }

                // 更新Header状态
                if (ViewModel.SelectedRows.Count <= 0)
                {
                    multiSelectButton.ChangeSelectStatus(SelectStatus.NotSelect);
                }
                else if (ViewModel.SelectedRows.Count < ViewModel.DataSource.Count)
                {
                    multiSelectButton.ChangeSelectStatus(SelectStatus.PartSelect);
                }
                else
                {
                    multiSelectButton.ChangeSelectStatus(SelectStatus.AllSelect);
                }
            };
            return checkBox;
        });

        selectColum.CellTemplate = dataTemplate;
        dataGrid.Columns.Insert(0, selectColum);
        ViewModel.DataSource.CollectionChanged += HandleCollectionChanged;
    }

    private void MultiSelectButtonClick(object? sender, RoutedEventArgs e)
    {
        var dataGrid = this.FindControl<DataGrid>("MainDataGrid");
        if (dataGrid == null || sender is not MultiSelectButton button)
        {
            return;
        }

        var visuals = dataGrid.GetVisualDescendants()
            .Where(it => it.GetType() == typeof(ToggleRadioButton))
            .Cast<ToggleRadioButton>()
            .ToList();
        var status = button.SelectStatus;

        switch (status)
        {
            case SelectStatus.NotSelect:
                visuals.ForEach(it => it.IsChecked = false);
                break;
            case SelectStatus.PartSelect:
                visuals.ForEach(it => it.IsChecked = true);
                break;
            case SelectStatus.AllSelect:
                visuals.ForEach(it => it.IsChecked = true);
                break;
        }
        
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        if (multiSelectButton != null)
        {
            multiSelectButton.Click -= MultiSelectButtonClick;
            multiSelectButton = null;
        }
        //
        // var dataGrid = this.FindControl<DataGrid>("MainDataGrid");
        // if (dataGrid != null)
        // {
        //     var dataGridColumns = dataGrid.Columns;
        //     var dataGridColumn = dataGridColumns[0];
        //     dataGridColumn.Header = null;
        //     dataGridColumns.RemoveAt(0);
        // }

        
       
        RemoveMultiColum();
        base.OnLoaded(e);
    }


    private void RemoveMultiColum()
    {
        ViewModel!.DataSource.CollectionChanged -= HandleCollectionChanged;
        
        ViewModel.DataSource.Clear();
        ViewModel.SelectedRows.Clear();
    }

    private void HandleCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (sender is not ObservableCollection<TS> collection)
        {
            return;
        }

        var removeItems = ViewModel!.SelectedRows.Except(collection);
        foreach (var removeItem in removeItems)
        {
            ViewModel.SelectedRows.Remove(removeItem);
        }
    }
}

public enum SelectedMode
{
    None = 0,
    Single,
    Multi
}