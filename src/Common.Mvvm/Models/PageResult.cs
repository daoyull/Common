namespace Common.Mvvm.Models;

public class PageResult<T>
{
    public int Total { get; set; }

    public List<T> List { get; set; }

    public PageResult(int total, List<T> list)
    {
        Total = total;
        List = list;
    }
}