namespace Common.Lib.Service;

public interface ILifePlugin
{
    Task OnCreated(ILifeCycle lifeCycle);

    Task OnLoaded(ILifeCycle lifeCycle);

    Task OnUnloaded(ILifeCycle lifeCycle);
}