﻿using System;

namespace ApiCatalogoJogos.Infrastructure.Model.ViewModel
{
    public class JogoViewModel : ViewModelBase
    {
        public Guid ProdutoraId { get; set; }
        public int Ano { get; set; }
        public int Valor { get; set; }
    }
}
