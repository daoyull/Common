using System.ComponentModel.DataAnnotations;

namespace Common.Lib.Exceptions;

public class DataVerifyException : Exception
{
    public List<ValidationResult> ErrorMessage { get; set; }

    public DataVerifyException(List<ValidationResult> errorMessage) : base(("数据验证失败"))
    {
        ErrorMessage = errorMessage;
    }
}