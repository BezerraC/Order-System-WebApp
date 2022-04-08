using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArkdBarV1.Models
{
    public class Pedido
    {
        public enum SituacaoPedido
        {
            Carrinho,
            Cancelado,
            Realizado,
            Varificado,
            Atendido,
            Entregue
        }

        [Key]
        [Display(Name = "Código")]
        public int IdPedido { get; set; }

        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        [Display(Name = "Data/Hora")]
        public DateTime DataHoraPedido { get; set; }

        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        [Display(Name = "Valor Total")]
        public double ValorTotal { get; set; }

        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        [DisplayName("Situação")]
        public SituacaoPedido Situacao { get; set; }

        public int? IdCliente { get; set; }

        public string IdCarrinho { get; set; }

        [ForeignKey("IdCliente")]
        public Cliente Cliente { get; }

        public DadosCliente DadosCliente { get; set; }

        public Endereco Endereco { get; set; }

        public ICollection<ItemPedido> ItensPedido { get; set; }
    }
}
