using System;

namespace Dio.CatalogoJogos.Api.Infrastructure.Authorization
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AllowAnonymousAttribute : Attribute
    {
    }
}
