using System;

namespace ApiCatalogoJogos.Infrastructure.Authorization
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AllowAnonymousAttribute : Attribute
    {
    }
}
