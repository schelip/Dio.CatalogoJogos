using System;
using System.Threading.Tasks;
using ApiCatalogoJogos.Business.Exceptions;
using ApiCatalogoJogos.Infrastructure.Model.InputModel;
using ApiCatalogoJogos.Infrastructure.Model.ViewModel;

namespace ApiCatalogoJogos.Business.Services
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
