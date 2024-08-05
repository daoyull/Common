using Common.Lib.Service;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Common.Mvvm.Abstracts;

public abstract class BaseAddDialogModel<T> : BaseViewModel where T : ObservableObject, new()
{
    public BaseAddDialogModel()
    {
        AddCommand = new(Execute);
    }

    private async Task Execute()
    {
        // 字段校验
        var verifyResult = await AddDataVerify();
        if (!verifyResult)
        {
            return;
        }

        // 新增处理
        await HandleAddCommand();
    }

    protected virtual async Task<bool> AddDataVerify()
    {
        if (Model is IDataVerify dataVerify)
        {
            var verifyResult = await dataVerify.Verify();
            if (!verifyResult.Item1)
            {
                return false;
            }
        }

        return true;
    }

    protected abstract Task HandleAddCommand();

    private T _model = new();

    /// <summary>
    /// 新增的模型
    /// </summary>
    public T Model
    {
        get => _model;
        set => SetProperty(ref _model, value);
    }

    #region Command

    public AsyncRelayCommand AddCommand { get; set; }

    #endregion
}