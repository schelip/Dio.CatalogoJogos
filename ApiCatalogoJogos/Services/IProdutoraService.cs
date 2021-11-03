using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiCatalogoJogos.Exceptions;
using ApiCatalogoJogos.Model.InputModel;
using ApiCatalogoJogos.Model.ViewModel;

namespace ApiCatalogoJogos.Services
{
    public interface IProdutoraService : IDisposable
    {
        /// <summary>
        /// Obtém uma determinada quantidade de produtoras do repositório a partir de um offset
        /// </summary>
        /// <param name="pagina">Offset (1 = nenhum offset)</param>
        /// <param name="quantidade">Quantidade de produtoras por página</param>
        /// <returns>Lista de produtoras obtidas</returns>
        Task<List<ProdutoraViewModel>> Obter(int pagina, int quantidade);
        /// <summary>
        /// Obtém todas as produtoras de um país no repositório
        /// </summary>
        /// <param name="isoPais">ISO de duas letras do país de origem das produtoras a serem obtidas</param>
        /// <returns>Lista das produtoras obtidas</returns>
        /// <exception cref="PaisInexistenteException">Se ocorrer um erro na validação do ISO</exception>
        Task<List<ProdutoraViewModel>> Obter(string isoPais);
        /// <summary>
        /// Obtém produtora do repositório pelo seu Id
        /// </summary>
        /// <param name="guid">Id da produtora a ser obtida</param>
        /// <returns>Dados da produtora obtida</returns>
        /// <exception cref="EntidadeNaoCadastradaException">Se a produtora mãe não for encontrada</exception>
        Task<ProdutoraViewModel> Obter(Guid guid);
        /// <summary>
        /// Insere produtora no repositório
        /// </summary>
        /// <param name="produtoraInput">Dados da produtora a ser inserida</param>
        /// <returns>Dados da produtora inserida</returns>
        /// <exception cref="EntidadeJaCadastradaException">Se foi encontrada um produtora conflitante</exception>
        /// <exception cref="PaisInexistenteException">Se ocorrer um erro na validação do ISO do Pais de origem</exception>
        Task<ProdutoraViewModel> Inserir(ProdutoraInputModel produtoraInput);
        /// <summary>
        /// Atualiza todos os dados de uma produtora no repositório
        /// </summary>
        /// <param name="id">Id da produtora a ser atualizada</param>
        /// <param name="produtoraInput">Novos dados da produtora</param>
        /// <returns>Dados atualizados da produtora</returns>
        /// <exception cref="EntidadeNaoCadastradaException">Se a produtora não foi encontrada</exception>
        Task<ProdutoraViewModel> Atualizar(Guid id, ProdutoraInputModel produtoraInput);
        /// <summary>
        /// Adicionar produtora filha a produtora no repositório
        /// </summary>
        /// <param name="idMae">Id da produtora mãe</param>
        /// <param name="idFilha">Id da produtora filha</param>
        /// <returns>Dados atualizados da rodutora mãe</returns>
        /// <exception cref="EntidadeNaoCadastradaException">Se uma das produtoras não for encontrada</exception>
        /// <exception cref="EntidadeJaCadastradaException">Se foi encontrada uma produtora conflitante entre as filhas já existentes</exception>
        Task<ProdutoraViewModel> Atualizar(Guid idMae, Guid idFilha);
        /// <summary>
        /// Remove produtora do repositório
        /// </summary>
        /// <param name="id">Id da produtora a ser removida</param>
        /// <returns></returns>
        Task Remover(Guid id);
    }
}
