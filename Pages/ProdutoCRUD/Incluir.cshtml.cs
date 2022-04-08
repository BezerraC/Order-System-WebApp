using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ArkdBarV1.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArkdBarV1.Pages.ProdutoCRUD
{
    public class IncluirModel : PageModel
    {

        [BindProperty]
        public Produto Produto { get; set; }

        private readonly ArkdBarV1.Data.ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public string CaminhoImagem { get; set; }

        [BindProperty]
        [Display(Name = "Imagem do produto")]
        [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
        public IFormFile ImagemProduto { get; set; }

        public IncluirModel(ArkdBarV1.Data.ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            CaminhoImagem = "~/img/produto/sem_imagem.jpeg";
        }

        public IActionResult OnGet()
        {
            return Page();
        }

       

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (ImagemProduto == null)
            {
                return Page();
            }

            var produto = new Produto();

            if (await TryUpdateModelAsync(produto, Produto.GetType(), nameof(Produto)))
            {
                _context.Produto.Add(Produto);
                await _context.SaveChangesAsync();
                await AppUtils.ProcessarArquivoDeImagem(Produto.IdProduto, ImagemProduto, _webHostEnvironment);

                return RedirectToPage("./Listar");
            }

            return Page();
           
        }
    }
}