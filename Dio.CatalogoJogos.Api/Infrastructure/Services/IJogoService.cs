using System;
using System.Threading.Tasks;
using Dio.CatalogoJogos.Api.Business.Exceptions;
using Dio.CatalogoJogos.Api.Web.Model.InputModel;
using Dio.CatalogoJogos.Api.Infrastructure.Model.ViewModel;

namespace Dio.CatalogoJogos.Api.Infrastructure.Services
{
    public interface IJogoService : IServiceBase<JogoInputModel, JogoViewModel>
    {
        /// <summary>
        /// Atualiza valor de um jogo
        /// </summary>
        /// <param name="id">Id do jogo a ser atualizado</param>
        /// <param name="valor">Novo valor</param>
        /// <returns>ViewModel atualizada do jogo</returns>
        /// <exception cref="EntidadeNaoCadastradaException"/>
        Task<JogoViewModel> AtualizarValor(Guid id, float valor);
    }
}
