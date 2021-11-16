using System;
using System.Threading.Tasks;
using Dio.CatalogoJogos.Api.Business.Exceptions;
using Dio.CatalogoJogos.Api.Web.Model.InputModel;
using Dio.CatalogoJogos.Api.Infrastructure.Model.ViewModel;

namespace Dio.CatalogoJogos.Api.Infrastructure.Services
{
    public interface IUsuarioService : IServiceBase<UsuarioInputModel, UsuarioViewModel>
    {
        /// <summary>
        /// Adiciona jogo à lista de jogos de um usuário
        /// </summary>
        /// <param name="idUsuario">Id do usuário</param>
        /// <param name="idJogo">Id do jogo</param>
        /// <returns>ViewModel atualizada do usuário</returns>
        /// <exception cref="EntidadeNaoCadastradaException"/>
        /// <exception cref="FundosInsuficientesException"/>
        Task<UsuarioViewModel> AdicionarJogo(Guid idUsuario, Guid idJogo);
        /// <summary>
        /// Atualiza os fundos de um usuário
        /// </summary>
        /// <param name="guid">Id do usuário</param>
        /// <param name="quant">Quantidade de fundos</param>
        /// <returns>ViewModel atualizada do usuário</returns>
        /// <exception cref="EntidadeNaoCadastradaException"/>
        Task<UsuarioViewModel> AtualizarFundos(Guid guid, float quant);
        /// <summary>
        /// Autentica usuario e retorna token jwt
        /// </summary>
        /// <param name="inputModel">LoginInputModel com os dados a serem validados</param>
        /// <returns>ViewModel da autenticacao</returns>
        /// <exception cref="ModelInvalidoException"/>
        Task<(string, UsuarioViewModel)> Autenticar(LoginInputModel inputModel);
    }
}
