using System;
using System.ComponentModel.DataAnnotations;

namespace ApiCatalogoJogos.Model.InputModel
{
    public class JogoInputModel
    {
        [Required(ErrorMessage = "É necessário informar o nome do jogo.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome do jogo deve conter entre 3 e 100 caracteres")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "É necessário informar a produtora do jogo.")]
        public Guid ProdutoraId { get; set; }
        public int Ano { get; set; } = -1;
    }
}
