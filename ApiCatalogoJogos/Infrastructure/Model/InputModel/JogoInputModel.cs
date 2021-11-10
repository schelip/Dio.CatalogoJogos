using System;
using System.ComponentModel.DataAnnotations;

namespace ApiCatalogoJogos.Infrastructure.Model.InputModel
{
    public class JogoInputModel : InputModelBase
    {
        [Required(ErrorMessage = "É necessário informar a produtora do jogo.")]
        public Guid ProdutoraId { get; set; }
        public int Ano { get; set; }
        [Required(ErrorMessage = "É necessário informar o valor do jogo")]
        public int Valor { get; set; }
    }
}
