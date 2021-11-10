﻿using System;
using System.Threading.Tasks;
using ApiCatalogoJogos.Infrastructure.Model.InputModel;
using ApiCatalogoJogos.Infrastructure.Model.ViewModel;

namespace ApiCatalogoJogos.Business.Services
{
    public interface IUsuarioService : IServiceBase<UsuarioInputModel, UsuarioViewModel>
    {
        /// <summary>
        /// Adiciona jogo á lista de jogos de um usuário
        /// </summary>
        /// <param name="idUsuario">Id do usuário</param>
        /// <param name="idJogo">Id do jogo</param>
        Task AdicionarJogo(Guid idUsuario, Guid idJogo);
        /// <summary>
        /// Atualiza os fundos de um usuário
        /// </summary>
        /// <param name="guid">Id do usuário</param>
        /// <param name="quant">Quantidade de fundos</param>
        /// <returns></returns>
        Task AtualizarFundos(Guid guid, float quant);
    }
}