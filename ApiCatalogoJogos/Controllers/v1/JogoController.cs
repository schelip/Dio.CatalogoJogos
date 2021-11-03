using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogoJogos.Business.Exceptions;
using ApiCatalogoJogos.Business.Services;
using ApiCatalogoJogos.Infrastructure.Model.InputModel;
using ApiCatalogoJogos.Infrastructure.Model.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ApiCatalogoJogos.Controllers.v1
{
    [Route("api/v1/jogos")]
    [ApiController]
    public class JogoController : ControllerBase
    {
        private readonly IJogoService _jogoService;

        public JogoController(IJogoService jogoService)
        {
            _jogoService = jogoService;
        }

        /// <summary>
        /// Obtém uma lista de jogos com uma quantidade e offset definidos
        /// </summary>
        /// <param name="pagina">Define o offset</param>
        /// <param name="quantidade">Define a quantidade em cada pagina</param>
        [SwaggerResponse(statusCode: 200, description: "Retorna jogos recuperados", Type = typeof(List<JogoViewModel>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhum jogo na página")]
        [SwaggerResponse(statusCode: 500, description: "Erro interno")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JogoViewModel>>> Obter(
            [FromQuery, Range(1, int.MaxValue)] int pagina = 1,
            [FromQuery, Range(1, 50)] int quantidade = 5)
        {
            var jogos = await _jogoService.Obter(pagina, quantidade);

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
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<JogoViewModel>> Obter([FromRoute] Guid id)
        {
            try
            {
                var jogo = await _jogoService.Obter(id);
                return Ok(jogo);
            }
            catch (EntidadeNaoCadastradaException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Insere novo jogo
        /// </summary>
        /// <param name="jogoInputModel">Jogo a ser inserido</param>
        [SwaggerResponse(statusCode: 200, description: "Retorna o jogo adicionado", Type = typeof(JogoViewModel))]
        [SwaggerResponse(statusCode: 422, description: "Erro durante a inserção")]
        [SwaggerResponse(statusCode: 500, description: "Erro interno")]
        [HttpPost]
        public async Task<ActionResult<JogoViewModel>> Inserir([FromBody] JogoInputModel jogoInputModel)
        {
            try
            {
                var jogo = await _jogoService.Inserir(jogoInputModel);

                return Created("", jogo);
            }
            catch (EntidadeJaCadastradaException)
            {
                return UnprocessableEntity("Já existe um jogo com este nome para esta produtora");
            }
        }

        /// <summary>
        /// Atualiza todos os campos de um jogo
        /// </summary>
        /// <param name="id">Id do jogo a ser atualizado</param>
        /// <param name="jogoInputModel">Jogo com novas características cofiguradas</param>
        [SwaggerResponse(statusCode: 200, description: "Retorna o jogo atualizado", Type = typeof(JogoViewModel))]
        [SwaggerResponse(statusCode: 404, description: "Jogo não encontrado")]
        [SwaggerResponse(statusCode: 500, description: "Erro interno")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Atualizar([FromRoute] Guid id, [FromBody] JogoInputModel jogoInputModel)
        {
            try
            {
                var jogoViewModel = await _jogoService.Atualizar(id, jogoInputModel);

                return Ok(jogoViewModel);
            }
            catch (EntidadeNaoCadastradaException)
            {
                return NotFound("Jogo não encontrado");
            }
        }

        /// <summary>
        /// Remove jogo
        /// </summary>
        /// <param name="id">Id do jogo a ser removido</param>
        [SwaggerResponse(statusCode: 200, description: "Retorna o id do jogo removido")]
        [SwaggerResponse(statusCode: 404, description: "Jogo não encontrado")]
        [SwaggerResponse(statusCode: 500, description: "Erro interno")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Remover([FromRoute] Guid id)
        {
            try
            {
                await _jogoService.Remover(id);

                return Ok($"Jogo de id {id} removido");
            }
            catch (EntidadeNaoCadastradaException)
            {
                return NotFound("Jogo não encontrado");
            }
        }
    }
}
