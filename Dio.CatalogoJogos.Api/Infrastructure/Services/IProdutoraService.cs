using System.Collections.Generic;
using System.Threading.Tasks;
using Dio.CatalogoJogos.Api.Web.Model.InputModel;
using Dio.CatalogoJogos.Api.Infrastructure.Model.ViewModel;

namespace Dio.CatalogoJogos.Api.Infrastructure.Services
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
