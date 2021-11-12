using System;
using System.Threading.Tasks;
using ApiCatalogoJogos.Business.Exceptions;
using ApiCatalogoJogos.Infrastructure.Model.InputModel;
using ApiCatalogoJogos.Infrastructure.Model.ViewModel;

namespace ApiCatalogoJogos.Business.Services
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
        Task<(string, UsuarioViewModel)> Autenticar(LoginInputModel inputModel);
        /// <summary>
        /// Obtém ViewModel de usuário a partir de seu email
        /// </summary>
        /// <param name="email">Email do usuário a ser obtido</param>
        /// <returns>ViewModel do usuário encontrado obtida</returns>
        /// <exception cref="EntidadeNaoCadastradaException"/>
        Task<UsuarioViewModel> Obter(string email);
    }
}
