using AspectCore.DynamicProxy;
using Common.Lib.Exceptions;
using Common.Lib.Service;

namespace Common.Lib.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class DataVerifyAttribute : AbstractInterceptorAttribute
{
    public override async Task Invoke(AspectContext context, AspectDelegate next)
    {
        try
        {
            foreach (var parameter in context.Parameters)
            {
                if (parameter is not IDataVerify dataVerify)
                {
                    continue;
                }

                var verifyResult = await dataVerify.Verify();
                if (verifyResult.Item1)
                {
                    continue;
                }

                throw new DataVerifyException(verifyResult.Item2);
            }

            Console.WriteLine("Before service call");
            await next(context);
        }
        catch (Exception)
        {
            Console.WriteLine("Service threw an exception!");
            throw;
        }
        finally
        {
            Console.WriteLine("After service call");
        }
    }
}