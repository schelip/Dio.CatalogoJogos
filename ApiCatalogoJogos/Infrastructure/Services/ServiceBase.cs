using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogoJogos.Business.Entities;
using ApiCatalogoJogos.Business.Exceptions;
using ApiCatalogoJogos.Business.Repositories;
using ApiCatalogoJogos.Business.Services;
using ApiCatalogoJogos.Infrastructure.Model;

namespace ApiCatalogoJogos.Infrastructure.Services
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

        public virtual async Task<List<TViewModel>> Obter(params (string, object)[] ps)
        {
            var entidades = await _repository.Obter(ps);

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

            var prms = ObterParametrosParaConflito(entidade);
            var entidadeConflitante = (await _repository.Obter(prms)).FirstOrDefault();

            if (entidadeConflitante != null)
            {
                var ex = new EntidadeJaCadastradaException();
                ex.Data.Add("entidadeConflitante", entidadeConflitante);
                throw ex;
            }

            await _repository.Inserir(entidade);

            return await ObterViewModel(entidade);
        }

        public virtual async Task<TViewModel> Atualizar(Guid id, TInputModel inputModel)
        {
            var entidade = await ObterEntidade(id, inputModel);

            await _repository.Atualizar(entidade);

            return await ObterViewModel(entidade);
        }
        
        public virtual async Task<TViewModel> Atualizar(Guid id, params (string, object)[] ps)
        {
            var entidade = await _repository.Obter(id);

            foreach (var param in ps)
            {
                var prop = typeof(TEntity).GetProperty(param.Item1);
                
                if (prop != null)
                    prop.SetValue(entidade, param.Item2);
            }

            await _repository.Atualizar(entidade);

            return await ObterViewModel(entidade);
        }

        public virtual async Task Remover(Guid id)
        {
            var entidade = await _repository.Obter(id);

            if (entidade == null)
                throw new EntidadeNaoCadastradaException();

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

        protected abstract Task<TEntity> ObterEntidade(Guid guid, TInputModel inputModel);
        protected abstract Task<TViewModel> ObterViewModel(TEntity entidade);
        protected abstract (string, object)[] ObterParametrosParaConflito(TEntity entidade);
    }
}
