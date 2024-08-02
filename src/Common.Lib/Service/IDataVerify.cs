using System.ComponentModel.DataAnnotations;
using Common.Lib.Helpers;

namespace Common.Lib.Service;

public interface IDataVerify
{
    public Task<(bool, List<ValidationResult>)> Verify()
    {
        if (!VerifyHelper.DataVerify(this, out var validationResults))
        {
            return Task.FromResult((false, validationResults));
        }

        return Task.FromResult((true, new List<ValidationResult>()));
    }
}