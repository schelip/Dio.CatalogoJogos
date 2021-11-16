using System;
using System.Net;
using System.Threading.Tasks;
using Dio.CatalogoJogos.Api.Business.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Dio.CatalogoJogos.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {

            var message = ex.Message;
            switch (ex)
            {
                case EntidadeNaoCadastradaException t:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case EntidadeJaCadastradaException t:
                    context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                    break;
                case FundosInsuficientesException t:
                    context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                    break;
                case ModelInvalidoException t2:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    message = "Ocorreu um erro durante sua solicitação, por favor, tente novamente mais tarde";
                    break;
            }

            context.Response.ContentType = "application/json";
            await context.Response
                .WriteAsync(
                JsonConvert.SerializeObject(
                    new { message = message }));
        }
    }
}
