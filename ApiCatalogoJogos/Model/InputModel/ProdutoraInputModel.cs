using System;
using System.ComponentModel.DataAnnotations;

namespace ApiCatalogoJogos.Model.InputModel
{
    public class ProdutoraInputModel
    {
        [Required(ErrorMessage = "É necessário informar o nome da Produtora")]
        public string Nome { get; set; }
        [StringLength(2, ErrorMessage = "O ISO do país deve ter no máximo 2 caracteres")]
        public string isoPais { get; set; }
        public Guid IdProdutoraMae { get; set; } 
    }
}
