namespace Common.Lib.Service;

public interface ILifePlugin
{
    Task OnCreate(ILifeCycle lifeCycle);
    
    Task OnInit(ILifeCycle lifeCycle);

    Task OnLoad(ILifeCycle lifeCycle);

    Task OnUnload(ILifeCycle lifeCycle);
}