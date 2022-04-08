using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ArkdBarV1.Models
{
    [Owned]
    public class Endereco
    {
        [Required(ErrorMessage = "O CEP informado não retornou um endereço válido.")]
        [MaxLength(100, ErrorMessage = "O campo {0} deve conter no máximo {1} caracteres.")]
        public string Logradouro { get; set; }

        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        [MaxLength(10, ErrorMessage = "O campo {0} deve conter no máximo {1} caracteres.")]
        [Display(Name = "Número")]
        public string Numero { get; set; }

        [MaxLength(100, ErrorMessage = "O campo {0} deve conter no máximo {1} caracteres.")]
        public string Complemento { get; set; }

        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório")]
        [MaxLength(50, ErrorMessage = "O campo {0} deve conter no máximo {1} caracteres.")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        [MaxLength(50, ErrorMessage = "O campo {0} deve conter no máximo {1} caracteres.")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        [MaxLength(2, ErrorMessage = "O campo {0} deve conter no máximo {1} caracteres.")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        [RegularExpression(@"[0-9]{8}$", ErrorMessage = "O campo {0} deve ser preenchido com um CEP válido.")]
        [MaxLength(8, ErrorMessage = "O campo {0} deve conter no máximo {1} caracteres.")]
        [UIHint("_CepTemplate")]
        public string CEP { get; set; }

        [MaxLength(100, ErrorMessage = "O campo {0} deve conter no máximo {1} caracteres.")]
        [Display(Name = "Referência")]
        public string Referencia { get; set; }



    }
}
