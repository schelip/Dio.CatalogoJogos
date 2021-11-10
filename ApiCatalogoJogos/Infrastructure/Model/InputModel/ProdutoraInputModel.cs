using System;
using System.ComponentModel.DataAnnotations;

namespace ApiCatalogoJogos.Infrastructure.Model.InputModel
{
    public class ProdutoraInputModel : InputModelBase
    {
        private Guid _produtoraMaeId;

        [StringLength(2, ErrorMessage = "O ISO do país deve ter no máximo 2 caracteres")]
        public string ISOPais { get; set; }
        public Guid? ProdutoraMaeId
        {
            get => _produtoraMaeId == Guid.Empty ? null : (Guid?)_produtoraMaeId;
            set => _produtoraMaeId = value == null ? Guid.Empty : value.Value;
        }
    }
}
