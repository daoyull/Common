namespace Common.Lib.Models;

public class PageResult<T>
{
    public long Total { get; set; }

    public List<T> List { get; set; }

    public PageResult(long total, List<T> list)
    {
        Total = total;
        List = list;
    }
}