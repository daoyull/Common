using System.ComponentModel;
using Common.Lib.Service;
using Common.Mvvm.Models;
using CommunityToolkit.Mvvm.Input;

namespace Common.Mvvm.Service;

public interface IPagination<T> where T : MvvmPageQuery
{
    public void BeginListener()
    {
        Pagination.PropertyChanged += PaginationOnPropertyChanged;
    }

    private void PaginationOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MvvmPageQuery.PageNum) || e.PropertyName == nameof(MvvmPageQuery.PageSize))
        {
            PageChangeCommand.Execute(Pagination);
        }
    }

    public void EndListener()
    {
        Pagination.PropertyChanged += PaginationOnPropertyChanged;
    }

    public T Pagination { get; }

    public AsyncRelayCommand<T> PageChangeCommand { get; set; }
}