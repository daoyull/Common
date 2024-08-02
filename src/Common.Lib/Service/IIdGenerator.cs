namespace Common.Lib.Service;

public interface IIdGenerator<T>
{
    T Generate();
}