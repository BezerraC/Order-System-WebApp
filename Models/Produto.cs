using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArkdBarV1.Models
{
    public class Produto
    {
        [Key]
        [Display(Name = "Código")]
        [DisplayFormat(DataFormatString = "{0:D6}")]
        public int IdProduto { get; set; }

        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        [MaxLength(100, ErrorMessage = "O campo {0} pode ter até {1} caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        [MaxLength(1000, ErrorMessage = "O campo {0} pode ter até {1} caracteres.")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
         
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        [DataType(DataType.Currency)]
        [Display(Name = "Preço")]
        public double? Preco { get; set; }

    }
}
