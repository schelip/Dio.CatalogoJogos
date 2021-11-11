using System.Collections.Generic;
using System.Threading.Tasks;
using ApiCatalogoJogos.Infrastructure.Model.InputModel;
using ApiCatalogoJogos.Infrastructure.Model.ViewModel;

namespace ApiCatalogoJogos.Business.Services
{
    public interface IProdutoraService : IServiceBase<ProdutoraInputModel, ProdutoraViewModel>
    {
        /// <summary>
        /// Obtém ViewModels de Produtoras de um país
        /// </summary>
        /// <param name="ISOPais">ISO de duas letras do país de origem</param>
        /// <returns>Lista de ViewModels obtidas</returns>
        Task<List<ProdutoraViewModel>> Obter(string ISOPais);
    }
}
