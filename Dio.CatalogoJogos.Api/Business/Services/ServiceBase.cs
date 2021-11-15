using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dio.CatalogoJogos.Api.Business.Entities;
using Dio.CatalogoJogos.Api.Business.Exceptions;
using Dio.CatalogoJogos.Api.Infrastructure.Data.Repositories;
using Dio.CatalogoJogos.Api.Infrastructure.Services;
using Dio.CatalogoJogos.Api.Web.Model;

namespace Dio.CatalogoJogos.Api.Business.Services
{
    public abstract class ServiceBase<TInputModel, TViewModel, TEntity> : IServiceBase<TInputModel, TViewModel>
        where TInputModel: InputModelBase
        where TViewModel: ViewModelBase
        where TEntity : EntityBase
    {
        protected readonly IRepositoryBase<TEntity> _repository;

        protected ServiceBase(IRepositoryBase<TEntity> repository)
        {
            _repository = repository;
        }

        public virtual async Task<List<TViewModel>> Obter(int pagina, int quantidade)
        {
            var entidades = await _repository.Obter(pagina, quantidade);

            return await ObterViewModels(entidades);
        }

        public virtual async Task<TViewModel> Obter(Guid id)
        {
            var entidade = await _repository.Obter(id);

            if (entidade == null)
                throw new EntidadeNaoCadastradaException();

            return await ObterViewModel(entidade);
        }

        public virtual async Task<TViewModel> Inserir(TInputModel inputModel)
        {
            var entidade = await ObterEntidade(Guid.Empty, inputModel);

            if (await VerificaConflito(entidade))
                throw new EntidadeJaCadastradaException();

            await _repository.Inserir(entidade);

            return await ObterViewModel(entidade);
        }

        public virtual async Task<TViewModel> Atualizar(Guid id, TInputModel inputModel)
        {
            var entidade = await ObterEntidade(id, inputModel);

            if (entidade == null)
                throw new EntidadeNaoCadastradaException(id);

            await _repository.Atualizar(entidade);

            return await ObterViewModel(entidade);
        }
        
        public virtual async Task Remover(Guid id)
        {
            var entidade = await _repository.Obter(id);

            if (entidade == null)
                throw new EntidadeNaoCadastradaException(id);

            await _repository.Remover(id);
        }
        
        public virtual void Dispose()
        {
            _repository?.Dispose();
        }

        protected async Task<List<TViewModel>> ObterViewModels(List<TEntity> entidades)
        {
            var list = new List<TViewModel>();

            foreach (var entidade in entidades)
                list.Add(await ObterViewModel(entidade));

            return list;
        }

        protected virtual async Task<bool> VerificaConflito(TEntity entidade)
        {
            return await _repository.VerificaConflito(entidade);
        }

        protected abstract Task<TEntity> ObterEntidade(Guid guid, TInputModel inputModel);
        protected abstract Task<TViewModel> ObterViewModel(TEntity entidade);
    }
}
