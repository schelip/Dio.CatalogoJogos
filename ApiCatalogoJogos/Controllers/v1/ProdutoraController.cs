using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogoJogos.Exceptions;
using ApiCatalogoJogos.Model.InputModel;
using ApiCatalogoJogos.Model.ViewModel;
using ApiCatalogoJogos.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ApiCatalogoJogos.Controllers.v1
{
    [Route("api/v1/produtoras")]
    [ApiController]
    public class ProdutoraController : ControllerBase
    {
        private readonly IProdutoraService _service;

        public ProdutoraController(IProdutoraService service)
        {
            _service = service;
        }

        /// <summary>
        /// Obtém uma lista de produtoras com uma quantidade e offset definidos
        /// </summary>
        /// <param name="pagina">Define o offset</param>
        /// <param name="quantidade">Define a quantidade em cada pagina</param>
        [SwaggerResponse(statusCode: 200, description: "Retorna produtoras recuperadas", Type = typeof(List<ProdutoraViewModel>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma produtora na página")]
        [SwaggerResponse(statusCode: 500, description: "Erro interno")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoraViewModel>>> Obter(
            [FromQuery, Range(1, int.MaxValue)] int pagina = 1,
            [FromQuery, Range(1, 50)] int quantidade = 5)
        {
            var produtoras = await _service.Obter(pagina, quantidade);

            if (produtoras.Count() == 0)
                return NoContent();

            return Ok(produtoras);
        }

        /// <summary>
        /// Obtém todas as produtoras de um país de origem
        /// </summary>
        /// <param name="isoPais">Nome do país de origem</param>
        [SwaggerResponse(statusCode: 200, description: "Retorna as produtoras obtidas", Type = typeof(List<ProdutoraViewModel>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma produtora desse país cadastrada")]
        [SwaggerResponse(statusCode: 500, description: "Erro interno")]
        [HttpGet("{isoPais:string}")]
        public async Task<ActionResult<List<ProdutoraViewModel>>> Obter([FromRoute] string isoPais)
        {
            try
            {
                var produtoras = await _service.Obter(isoPais);

                if (produtoras.Count == 0)
                    return NoContent();

                return Ok(produtoras);
            }
            catch (PaisInexistenteException ex)
            {
                return UnprocessableEntity(ex);
            }
        }

        /// <summary>
        /// Obtém produtora a partir de seu Id
        /// </summary>
        /// <param name="id">Id da produtora</param>
        [SwaggerResponse(statusCode: 200, description: "Retorna a produtora com id informado", Type = typeof(ProdutoraViewModel))]
        [SwaggerResponse(statusCode: 404, description: "Produtora não encontrada")]
        [SwaggerResponse(statusCode: 500, description: "Erro interno")]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProdutoraViewModel>> Obter([FromRoute] Guid id)
        {
            try
            {
                var produtora = await _service.Obter(id);
                return Ok(produtora);
            }
            catch (EntidadeNaoCadastradaException ex)
            {
                return NotFound(ex);
            }
        }

        /// <summary>
        /// Obtém produtoras filhas de uma produtora
        /// </summary>
        /// <param name="id">Id da produtora mãe</param>
        [SwaggerResponse(statusCode: 200, description: "Retorna as produtoras obtidas", Type = typeof(List<ProdutoraViewModel>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma produtora filha cadastrada")]
        [SwaggerResponse(statusCode: 404, description: "Produtora mãe não encontrada")]
        [SwaggerResponse(statusCode: 500, description: "Erro interno")]
        [HttpGet("{id:guid}/filhas")]
        public async Task<ActionResult<List<ProdutoraViewModel>>> ObterFilhas([FromRoute] Guid id)
        {
            try
            {
                var filhas = await _service.ObterFilhas(id);

                if (filhas.Count() == 0)
                    return NoContent();
                
                return Ok(filhas);
            }
            catch(EntidadeNaoCadastradaException ex)
            {
                return NotFound(ex);
            }
        }

        /// <summary>
        /// Insere nova produtora
        /// </summary>
        /// <param name="produtoraInput">Produtora a ser inserida</param>
        [SwaggerResponse(statusCode: 200, description: "Retorna a produtora inserida", Type = typeof(ProdutoraViewModel))]
        [SwaggerResponse(statusCode: 422, description: "Erro durante a inserção")]
        [SwaggerResponse(statusCode: 500, description: "Erro interno")]
        [HttpPost]
        public async Task<ActionResult<JogoViewModel>> Inserir([FromBody] ProdutoraInputModel produtoraInput)
        {
            try
            {
                var produtora = await _service.Inserir(produtoraInput);

                return Created("", produtora);
            }
            catch (Exception ex) when (ex is PaisInexistenteException || ex is EntidadeJaCadastradaException)
            {
                return UnprocessableEntity(ex);
            }
        }

        /// <summary>
        /// Atualiza todos os campos de uma produtora
        /// </summary>
        /// <param name="id">Id da produtora a ser atualizada</param>
        /// <param name="produtoraInput">Produtora com as novas características configuradas</param>
        [SwaggerResponse(statusCode: 200, description: "Retorna a produtora atualizada", Type = typeof(ProdutoraViewModel))]
        [SwaggerResponse(statusCode: 404, description: "Produtora não encontrada")]
        [SwaggerResponse(statusCode: 500, description: "Erro interno")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Atualizar([FromRoute] Guid id, [FromBody] ProdutoraInputModel produtoraInput)
        {
            try
            {
                var produtora = await _service.Atualizar(id, produtoraInput);

                return Ok(produtora);
            }
            catch (EntidadeNaoCadastradaException ex)
            {
                return NotFound(ex);
            }
        }

        /// <summary>
        /// Remove produtora
        /// </summary>
        /// <param name="id">Id da produtora a ser removida</param>
        [SwaggerResponse(statusCode: 200, description: "Retorna o id da produtora removida")]
        [SwaggerResponse(statusCode: 404, description: "Produtora não encontrada")]
        [SwaggerResponse(statusCode: 500, description: "Erro interno")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Remover([FromRoute] Guid id)
        {
            try
            {
                await _service.Remover(id);

                return Ok($"Produtora de id {id} removida");
            }
            catch (EntidadeNaoCadastradaException ex)
            {
                return NotFound(ex);
            }
        }
    }
}

