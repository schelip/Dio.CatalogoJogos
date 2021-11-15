using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dio.CatalogoJogos.Api.Business.Exceptions;
using Dio.CatalogoJogos.Api.Web.Model;

namespace Dio.CatalogoJogos.Api.Infrastructure.Services
{
    /// <summary>
    /// Contrato para classe que gerencia operações no repositório utilizando Models
    /// </summary>
    public interface IServiceBase<TInputModel, TViewModel> : IDisposable
        where TInputModel : InputModelBase
        where TViewModel : ViewModelBase
    {
        /// <summary>
        /// Obtém uma quantidade definida de ViewModels de entidades a partir de um offset
        /// </summary>
        /// <param name="pagina">Offset (1 = nenhum offset)</param>
        /// <param name="quantidade">Quantidade de ViewModels por página</param>
        /// <returns>Lista das ViewModels obtidas</returns>
        Task<List<TViewModel>> Obter(int pagina, int quantidade);
        /// <summary>
        /// Obtém ViewModel de entidade do respositório pelo Id
        /// </summary>
        /// <param name="id">Id da entidade a ser obtida</param>
        /// <returns>ViewModel obtida</returns>
        /// <exception cref="EntidadeNaoCadastradaException">Se não existe uma entidade cadastrada com esse id</exception>
        Task<TViewModel> Obter(Guid id);
        /// <summary>
        /// Insere entidade no repositório a partir de uma InputModel
        /// </summary>
        /// <param name="inputModel">InputModel da entidade a ser inserida</param>
        /// <returns>ViewModel da entidade inserida</returns>
        /// <exception cref="EntidadeJaCadastradaException">Se existe um entidade conflitante já cadastrada</exception>
        Task<TViewModel> Inserir(TInputModel inputModel);
        /// <summary>
        /// Atualiza todos os dados de uma entidade cadastrada para os dados de uma InputModel
        /// </summary>
        /// <param name="id">Id do entidade a ser atualizado</param>
        /// <param name="inputModel">InputModel com novos dados para a entidade</param>
        /// <returns>ViewModel da entidade atualizada</returns>
        /// <exception cref="EntidadeNaoCadastradaException">Se não existe uma entidade com esse id cadastrada</exception>
        Task<TViewModel> Atualizar(Guid id, TInputModel inputModel);
        /// <summary>
        /// Remove entidade do repositório
        /// </summary>
        /// <param name="id">Id da entidade a ser removida</param>
        /// <returns></returns>
        /// <exception cref="EntidadeNaoCadastradaException">Se não existe uma entidade com esse id cadastrada</exception>
        Task Remover(Guid id);
    }
}