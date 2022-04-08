using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArkdBarV1.Models
{
    public class ItemPedido
    {
        [Required]
        public int IdPedido { get; set; }

        [Required]
        public int IdProduto { get; set; }

        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        public float Quantidade { get; set; }

        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        [Display(Name = "Valor Unitário")]
        public double ValorUnitario { get; set; }

        [NotMapped]
        [Display(Name = "Valor Item")]
        public double ValorItem
        {
            get
            {
                return Quantidade * ValorUnitario;
            }
        }

        [ForeignKey("IdPedido")]
        public Pedido Pedido { get; set; }

        [ForeignKey("IdProduto")]
        public Produto Produto { get; set; }



    }
}
