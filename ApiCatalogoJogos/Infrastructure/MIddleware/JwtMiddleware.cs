﻿using System.Linq;
using System.Threading.Tasks;
using Dio.CatalogoJogos.Api.Business.Services;
using Dio.CatalogoJogos.Api.Infrastructure.Authorization;
using Microsoft.AspNetCore.Http;

namespace Dio.CatalogoJogos.Api.Infrastructure.MIddleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUsuarioService service, IJwtUtils utils)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var id = utils.ValidarJwtToken(token);

            if (id != null)
                context.Items["Usuario"] = await service.Obter(id.Value);

            await _next(context);
        }
    }
}
