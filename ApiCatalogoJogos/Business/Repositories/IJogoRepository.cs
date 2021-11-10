using System;
using ApiCatalogoJogos.Business.Entities;

namespace ApiCatalogoJogos.Business.Repositories
{
    public interface IJogoRepository : IRepositoryBase<Jogo>, IDisposable
    {
    }
}
