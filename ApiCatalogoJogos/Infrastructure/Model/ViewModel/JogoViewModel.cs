﻿using System;

namespace Dio.CatalogoJogos.Api.Infrastructure.Model.ViewModel
{
    public class JogoViewModel : ViewModelBase
    {
        public Guid ProdutoraId { get; set; }
        public int Ano { get; set; }
        public float Valor { get; set; }
    }
}
