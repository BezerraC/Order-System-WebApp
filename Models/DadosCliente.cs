using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ArkdBarV1.Models
{
    [Owned]
    public class DadosCliente
    {
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        [MaxLength(100, ErrorMessage = "O campo {0} deve conter no máximo {1} caracteres.")]
        public string NomeDoCliente { get; set; }

        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        public string MesaDoCliente { get; set; }
    }
}
