using System;
using ApiCatalogoJogos.Infrastructure.Model.InputModel;
using ApiCatalogoJogos.Infrastructure.Model.ViewModel;

namespace ApiCatalogoJogos.Business.Services
{
    public interface IJogoService : IServiceBase<JogoInputModel, JogoViewModel>, IDisposable
    {
    }
}
