using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Dio.CatalogoJogos.Api.Business.Exceptions;
using Dio.CatalogoJogos.Api.Infrastructure.Model.ViewModel;
using Dio.CatalogoJogos.Api.Infrastructure.Services;
using Dio.CatalogoJogos.Api.Web.Model.InputModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Dio.CatalogoJogos.Api.Web.Controllers.v1
{
    [Authorize(Roles = "Moderador,Administrador")]
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
        [AllowAnonymous]
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
        [AllowAnonymous]
        [HttpGet("{isoPais}")]
        public async Task<ActionResult<List<ProdutoraViewModel>>> Obter([FromRoute] string isoPais)
        {
            var produtoras = await _service.Obter(isoPais);

            if (produtoras.Count == 0)
                return NoContent();

            return Ok(produtoras);
        }

        /// <summary>
        /// Obtém produtora a partir de seu Id
        /// </summary>
        /// <param name="id">Id da produtora</param>
        [SwaggerResponse(statusCode: 200, description: "Retorna a produtora com id informado", Type = typeof(ProdutoraViewModel))]
        [SwaggerResponse(statusCode: 404, description: "Produtora não encontrada")]
        [SwaggerResponse(statusCode: 500, description: "Erro interno")]
        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProdutoraViewModel>> Obter([FromRoute] Guid id)
        {
            var produtora = await _service.Obter(id);
            return Ok(produtora);
        }

        /// <summary>
        /// Insere nova produtora
        /// </summary>
        /// <param name="produtoraInput">Produtora a ser inserida</param>
        [SwaggerResponse(statusCode: 201, description: "Retorna a produtora inserida", Type = typeof(ProdutoraViewModel))]
        [SwaggerResponse(statusCode: 400, description: "Erro nos dados informados")]
        [SwaggerResponse(statusCode: 422, description: "Erro durante a inserção")]
        [SwaggerResponse(statusCode: 500, description: "Erro interno")]
        [HttpPost]
        public async Task<ActionResult<JogoViewModel>> Inserir([FromBody] ProdutoraInputModel produtoraInput)
        {
            var produtora = await _service.Inserir(produtoraInput);
            return Created("", produtora);
        }

        /// <summary>
        /// Atualiza todos os campos de uma produtora
        /// </summary>
        /// <param name="id">Id da produtora a ser atualizada</param>
        /// <param name="produtoraInput">Produtora com as novas características configuradas</param>
        [SwaggerResponse(statusCode: 200, description: "Retorna a produtora atualizada", Type = typeof(ProdutoraViewModel))]
        [SwaggerResponse(statusCode: 400, description: "Erro nos dados informados")]
        [SwaggerResponse(statusCode: 404, description: "Produtora não encontrada")]
        [SwaggerResponse(statusCode: 500, description: "Erro interno")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ProdutoraViewModel>> Atualizar([FromRoute] Guid id, [FromBody] ProdutoraInputModel produtoraInput)
        {
            var produtora = await _service.Atualizar(id, produtoraInput);
            return Ok(produtora);
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
            await _service.Remover(id);
            return Ok($"Produtora de id {id} removida");
        }
    }
}

