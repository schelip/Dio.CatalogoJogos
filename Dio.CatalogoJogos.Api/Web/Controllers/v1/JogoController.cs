using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Dio.CatalogoJogos.Api.Business.Exceptions;
using Dio.CatalogoJogos.Api.Enum;
using Dio.CatalogoJogos.Api.Infrastructure.Authorization;
using Dio.CatalogoJogos.Api.Web.Model.InputModel;
using Dio.CatalogoJogos.Api.Infrastructure.Model.ViewModel;
using Dio.CatalogoJogos.Api.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Dio.CatalogoJogos.Api.Web.Controllers.v1
{
    [Authorize(PermissaoUsuario.Moderador, PermissaoUsuario.Administrador)]
    [Route("api/v1/jogos")]
    [ApiController]
    public class JogoController : ControllerBase
    {
        private readonly IJogoService _service;

        public JogoController(IJogoService service)
        {
            _service = service;
        }

        /// <summary>
        /// Obtém uma lista de jogos com uma quantidade e offset definidos
        /// </summary>
        /// <param name="pagina">Define o offset</param>
        /// <param name="quantidade">Define a quantidade em cada pagina</param>
        [SwaggerResponse(statusCode: 200, description: "Retorna jogos recuperados", Type = typeof(List<JogoViewModel>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhum jogo na página")]
        [SwaggerResponse(statusCode: 500, description: "Erro interno")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JogoViewModel>>> Obter(
            [FromQuery, Range(1, int.MaxValue)] int pagina = 1,
            [FromQuery, Range(1, 50)] int quantidade = 5)
        {
            var jogos = await _service.Obter(pagina, quantidade);

            if (jogos.Count() == 0)
                return NoContent();

            return Ok(jogos);
        }

        /// <summary>
        /// Obtém jogo a partir de seu Id
        /// </summary>
        /// <param name="id">Id do jogo</param>
        [SwaggerResponse(statusCode: 200, description: "Retorna o jogo com id informado", Type = typeof(JogoViewModel))]
        [SwaggerResponse(statusCode: 404, description: "Jogo não encontrado")]
        [SwaggerResponse(statusCode: 500, description: "Erro interno")]
        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<JogoViewModel>> Obter([FromRoute] Guid id)
        {
            try
            {
                var jogo = await _service.Obter(id);
                return Ok(jogo);
            }
            catch (EntidadeNaoCadastradaException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Insere novo jogo
        /// </summary>
        /// <param name="jogoInputModel">Jogo a ser inserido</param>
        [SwaggerResponse(statusCode: 200, description: "Retorna o jogo adicionado", Type = typeof(JogoViewModel))]
        [SwaggerResponse(statusCode: 401, description: "Permissão insuficiente")]
        [SwaggerResponse(statusCode: 422, description: "Erro durante a inserção")]
        [SwaggerResponse(statusCode: 500, description: "Erro interno")]
        [HttpPost]
        public async Task<ActionResult<JogoViewModel>> Inserir([FromBody] JogoInputModel jogoInputModel)
        {
            try
            {
                var jogo = await _service.Inserir(jogoInputModel);

                return Created("", jogo);
            }
            catch (EntidadeJaCadastradaException ex)
            {
                return UnprocessableEntity(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza todos os campos de um jogo
        /// </summary>
        /// <param name="id">Id do jogo a ser atualizado</param>
        /// <param name="jogoInputModel">Jogo com novas características cofiguradas</param>
        [SwaggerResponse(statusCode: 200, description: "Retorna o jogo atualizado", Type = typeof(JogoViewModel))]
        [SwaggerResponse(statusCode: 401, description: "Permissão insuficiente")]
        [SwaggerResponse(statusCode: 404, description: "Jogo não encontrado")]
        [SwaggerResponse(statusCode: 500, description: "Erro interno")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<JogoViewModel>> Atualizar([FromRoute] Guid id, [FromBody] JogoInputModel jogoInputModel)
        {
            try
            {
                var jogoViewModel = await _service.Atualizar(id, jogoInputModel);

                return Ok(jogoViewModel);
            }
            catch (EntidadeNaoCadastradaException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza valor de um jogo
        /// </summary>
        /// <param name="id">Id do jogo a ser atualizado</param>
        /// <param name="quant">Novo valor</param>
        [SwaggerResponse(statusCode: 200, description: "Retorna o jogo atualizado", Type = typeof(JogoViewModel))]
        [SwaggerResponse(statusCode: 401, description: "Permissão insuficiente")]
        [SwaggerResponse(statusCode: 400, description: "Erro nos dados informados")]
        [SwaggerResponse(statusCode: 404, description: "Jogo não encontrado")]
        [SwaggerResponse(statusCode: 500, description: "Erro interno")]
        [HttpPatch("{id:guid}/valor/{quant:float}")]
        public async Task<ActionResult<JogoViewModel>> Atualizar([FromRoute] Guid id, [FromRoute] float quant)
        {
            try
            {
                var jogoViewModel = await _service.AtualizarValor(id, quant);

                return Ok(jogoViewModel);
            }
            catch (EntidadeNaoCadastradaException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Remove jogo
        /// </summary>
        /// <param name="id">Id do jogo a ser removido</param>
        [SwaggerResponse(statusCode: 200, description: "Retorna o id do jogo removido")]
        [SwaggerResponse(statusCode: 401, description: "Permissão insuficiente")]
        [SwaggerResponse(statusCode: 404, description: "Jogo não encontrado")]
        [SwaggerResponse(statusCode: 500, description: "Erro interno")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Remover([FromRoute] Guid id)
        {
            try
            {
                await _service.Remover(id);

                return Ok($"Jogo de id {id} removido");
            }
            catch (EntidadeNaoCadastradaException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
