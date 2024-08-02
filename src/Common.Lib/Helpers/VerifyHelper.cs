using System.ComponentModel.DataAnnotations;

namespace Common.Lib.Helpers;

public static class VerifyHelper
{
    public static bool DataVerify(object o, out List<ValidationResult> results)
    {
        var list = new List<ValidationResult>();
        var b = Validator.TryValidateObject(o, new ValidationContext(o), list);
        results = list;
        return b;
    }
}