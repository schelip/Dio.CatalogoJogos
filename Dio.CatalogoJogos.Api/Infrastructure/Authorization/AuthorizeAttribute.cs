using System;
using System.Collections.Generic;
using System.Linq;
using Dio.CatalogoJogos.Api.Enum;
using Dio.CatalogoJogos.Api.Infrastructure.Model.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dio.CatalogoJogos.Api.Infrastructure.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IList<PermissaoUsuario> _permissoes;

        public AuthorizeAttribute(params PermissaoUsuario[] permissoes)
        {
            _permissoes = permissoes ?? new PermissaoUsuario[] { };
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Permitir anonimo
            if (context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any())
                return;

            var usuario = (UsuarioViewModel)context.HttpContext.Items["Usuario"];
            if (usuario == null || _permissoes.Any() && !_permissoes.Contains(usuario.Permissao))
                context.Result = new JsonResult(new { message = "Não autorizado" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}
