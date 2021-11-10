using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogoJogos.Business.Entities.Named;
using ApiCatalogoJogos.Business.Repositories;
using ApiCatalogoJogos.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogoJogos.Infrastructure.Data.Repositories
{
    public class ProdutoraRepository : RepositoryBase<Produtora>, IProdutoraRepository
    {
        public ProdutoraRepository(CatalogoJogosDbContext context) : base(context)
        {
        }
    }
}
