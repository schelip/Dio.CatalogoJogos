using System;
using System.ComponentModel.DataAnnotations;

namespace ApiCatalogoJogos.Model.InputModel
{
    public class ProdutoraInputModel
    {
        private Guid _produtoraMaeId;

        [Required(ErrorMessage = "É necessário informar o nome da Produtora")]
        public string Nome { get; set; }
        [StringLength(2, ErrorMessage = "O ISO do país deve ter no máximo 2 caracteres")]
        public string ISOPais { get; set; }
        public Guid? ProdutoraMaeId
        {
            get => _produtoraMaeId == Guid.Empty ? null : (Guid?)_produtoraMaeId;
            set => _produtoraMaeId = value == null ? Guid.Empty : value.Value;
        }
    }
}
