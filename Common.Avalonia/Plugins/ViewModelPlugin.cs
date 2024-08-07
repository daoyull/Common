using Autofac;
using Common.Avalonia.Abstracts;
using Common.Lib.Ioc;
using Common.Lib.Service;
using Common.Mvvm.Abstracts;

namespace Common.Avalonia.Plugins;

internal class ViewModelPlugin<T> : ILifePlugin where T : BaseViewModel
{
    public Task OnCreated(ILifeCycle lifeCycle)
    {
        if (lifeCycle is not UserComponent<T> userComponent) return Task.CompletedTask;
        try
        {
            // Ioc加载ViewModel
            // 设计期加载异常
            var viewModel = Ioc.IsBuilder ? Ioc.Container?.ResolveOptional<T>() : default;
            userComponent.ViewModel = viewModel;
            userComponent.DataContext = viewModel;
            viewModel?.OnCreated();
        }
        catch (Exception e)
        {
            // Ignore
            Console.WriteLine(e.Message);
        }

        return Task.CompletedTask;
    }

    public Task OnLoaded(ILifeCycle lifeCycle)
    {
        if (lifeCycle is not UserComponent<T> userComponent) return Task.CompletedTask;
        userComponent.ViewModel?.OnLoaded();
        return Task.CompletedTask;
    }

    public Task OnUnloaded(ILifeCycle lifeCycle)
    {
        if (lifeCycle is not UserComponent<T> userComponent) return Task.CompletedTask;
        userComponent.ViewModel?.OnUnloaded();
        return Task.CompletedTask;
    }
}