using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogoJogos.Business.Entities.Named;
using ApiCatalogoJogos.Business.Exceptions;
using ApiCatalogoJogos.Business.Services;
using ApiCatalogoJogos.Enum;
using ApiCatalogoJogos.Infrastructure.Authorization;
using ApiCatalogoJogos.Infrastructure.Model.InputModel;
using ApiCatalogoJogos.Infrastructure.Model.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ApiCatalogoJogos.Controllers.v1
{
    [Authorize(PermissaoUsuario.Usuario, PermissaoUsuario.Administrador)]
    [Route("api/v1/usuarios")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _service;

        public UsuarioController(IUsuarioService service)
        {
            _service = service;
        }

        /// <summary>
        /// Obtém uma lista de usuários com uma quantidade e offset definidos
        /// </summary>
        /// <param name="pagina">Define o offset</param>
        /// <param name="quantidade">Define a quantidade em cada pagina</param>
        [SwaggerResponse(statusCode: 200, description: "Retorna usuários recuperados", Type = typeof(List<UsuarioViewModel>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhum usuário na página")]
        [SwaggerResponse(statusCode: 401, description: "Permissão insuficiente")]
        [SwaggerResponse(statusCode: 500, description: "Erro interno")]
        [Authorize(PermissaoUsuario.Administrador)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioViewModel>>> Obter(
            [FromQuery, Range(1, int.MaxValue)] int pagina = 1,
            [FromQuery, Range(1, 50)] int quantidade = 5)
        {
            var usuarios = await _service.Obter(pagina, quantidade);

            if (usuarios.Count() == 0)
                return NoContent();

            return Ok(usuarios);
        }

        /// <summary>
        /// Obtém usuário a partir de seu Id
        /// </summary>
        /// <param name="id">Id do usuário</param>
        [SwaggerResponse(statusCode: 200, description: "Retorna o usuário com id informado", Type = typeof(UsuarioViewModel))]
        [SwaggerResponse(statusCode: 401, description: "Permissão insuficiente")]
        [SwaggerResponse(statusCode: 404, description: "Usuário não encontrado")]
        [SwaggerResponse(statusCode: 500, description: "Erro interno")]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UsuarioViewModel>> Obter([FromRoute] Guid id)
        {
            try
            {
                if (!ValidaSessao(id))
                    return Unauthorized();

                var usuario = await _service.Obter(id);
                return Ok(usuario);
            }
            catch (EntidadeNaoCadastradaException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Insere novo usuário
        /// </summary>
        /// <param name="usuarioInput">Usuário a ser inserido</param>
        [SwaggerResponse(statusCode: 201, description: "Retorna o usuário inserido", Type = typeof(UsuarioViewModel))]
        [SwaggerResponse(statusCode: 400, description: "Erro nos dados informados")]
        [SwaggerResponse(statusCode: 422, description: "Erro durante a inserção")]
        [SwaggerResponse(statusCode: 500, description: "Erro interno")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<JogoViewModel>> Inserir([FromBody] UsuarioInputModel usuarioInput)
        {
            try
            {
                var usuario = await _service.Inserir(usuarioInput);

                return Created("", usuario);
            }
            catch (Exception ex) when (ex is PaisInexistenteException || ex is EntidadeNaoCadastradaException)
            {
                return UnprocessableEntity(ex.Message);
            }
            catch (EntidadeJaCadastradaException ex)
            {
                return UnprocessableEntity(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza todos os campos de um usuário
        /// </summary>
        /// <param name="id">Id do usuário a ser atualizado</param>
        /// <param name="usuarioInput">Usuário com as novas características configuradas</param>
        [SwaggerResponse(statusCode: 200, description: "Retorna o usuário atualizado", Type = typeof(UsuarioViewModel))]
        [SwaggerResponse(statusCode: 401, description: "Permissão insuficiente")]
        [SwaggerResponse(statusCode: 400, description: "Erro nos dados informados")]
        [SwaggerResponse(statusCode: 404, description: "Usuário não encontrado")]
        [SwaggerResponse(statusCode: 500, description: "Erro interno")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Atualizar([FromRoute] Guid id, [FromBody] UsuarioInputModel usuarioInput)
        {
            try
            {
                if (!ValidaSessao(id))
                    return Unauthorized();

                var usuario = await _service.Atualizar(id, usuarioInput);

                return Ok(usuario);
            }
            catch (EntidadeNaoCadastradaException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza fundos de um usuário
        /// </summary>
        /// <param name="id">Id do usuário a ser atualizado</param>
        /// <param name="quant">Nova quantia de fundos</param>
        [SwaggerResponse(statusCode: 200, description: "Retorna o usuário atualizado", Type = typeof(UsuarioViewModel))]
        [SwaggerResponse(statusCode: 401, description: "Permissão insuficiente")]
        [SwaggerResponse(statusCode: 400, description: "Erro nos dados informados")]
        [SwaggerResponse(statusCode: 404, description: "Usuário não encontrado")]
        [SwaggerResponse(statusCode: 500, description: "Erro interno")]
        [HttpPatch("{id:guid}/fundos/{quant:float}")]
        public async Task<ActionResult> Atualizar([FromRoute] Guid id, [FromRoute] float quant)
        {
            try
            {
                if (!ValidaSessao(id))
                    return Unauthorized();

                var usuario = await _service.AtualizarFundos(id, quant);

                return Ok(usuario);
            }
            catch (EntidadeNaoCadastradaException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Adiciona jogo à lista de jogos do usuário
        /// </summary>
        /// <param name="id">Id do usuário</param>
        /// <param name="idJogo">Id do jogo a ser adicionado</param>
        [SwaggerResponse(statusCode: 200, description: "Retorna o usuário atualizado", Type = typeof(UsuarioViewModel))]
        [SwaggerResponse(statusCode: 401, description: "Permissão insuficiente")]
        [SwaggerResponse(statusCode: 400, description: "Erro nos dados informados")]
        [SwaggerResponse(statusCode: 404, description: "Usuário não encontrado")]
        [SwaggerResponse(statusCode: 500, description: "Erro interno")]
        [HttpPatch("{id:guid}/jogos/{idJogo:guid}")]
        public async Task<ActionResult> Atualizar([FromRoute] Guid id, [FromRoute] Guid idJogo)
        {
            try
            {
                if (!ValidaSessao(id))
                    return Unauthorized();

                var usuario = await _service.AdicionarJogo(id, idJogo);

                return Ok(usuario);
            }
            catch (EntidadeNaoCadastradaException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Remove usuário
        /// </summary>
        /// <param name="id">Id do usuário a ser removido</param>
        [SwaggerResponse(statusCode: 200, description: "Retorna o id do usuário removido")]
        [SwaggerResponse(statusCode: 401, description: "Permissão insuficiente")]
        [SwaggerResponse(statusCode: 404, description: "Usuário não encontrado")]
        [SwaggerResponse(statusCode: 500, description: "Erro interno")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Remover([FromRoute] Guid id)
        {
            try
            {
                if (!ValidaSessao(id))
                    return Unauthorized();

                await _service.Remover(id);

                return Ok(id);
            }
            catch (EntidadeNaoCadastradaException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Autentica usuário cadastrado
        /// </summary>
        /// <param name="inputModel">InputModel contendo dados para login</param>
        [SwaggerResponse(statusCode: 200, description: "Retorna o token de autenticação e ViewModel do usário autenticado")]
        [SwaggerResponse(statusCode: 404, description: "Usuário não encontrado")]
        [SwaggerResponse(statusCode: 500, description: "Erro interno")]
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Autenticar(LoginInputModel inputModel)
        {
            try
            {
                var auth = await _service.Autenticar(inputModel);
                return Ok(new JsonResult(new {token = auth.Item1, usuario = auth.Item2 }).Value);
            }
            catch (AutorizacaoException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        // Util
        private bool ValidaSessao(Guid id)
        {
            var usuario = (UsuarioViewModel)HttpContext.Items["Usuario"];

            return (id == usuario.Id || usuario.Permissao == PermissaoUsuario.Administrador);
        }
    }
}
