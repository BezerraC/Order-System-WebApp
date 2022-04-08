using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ArkdBarV1.Data;
using ArkdBarV1.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ArkdBarV1.Pages.ProdutoCRUD
{
    public class AlterarModel : PageModel
    {
        private readonly ArkdBarV1.Data.ApplicationDbContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;

        [BindProperty]
        public Produto Produto { get; set; }

        public string CaminhoImagem { get; set; }

        [BindProperty]
        [Display(Name = "Imagem do Produto")]
        public IFormFile ImagemProduto { get; set; }

        public AlterarModel(ArkdBarV1.Data.ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;

        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Produto = await _context.Produto.FirstOrDefaultAsync(p => p.IdProduto == id);

            if (Produto == null)
            {
                return NotFound();
            }

            CaminhoImagem = $"~/img/produto/{Produto.IdProduto:D6}.jpeg";

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Produto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                //se há uma imagem de produto submetida
                if (ImagemProduto != null)
                     await AppUtils.ProcessarArquivoDeImagem(Produto.IdProduto,
                        ImagemProduto, _webHostEnvironment);
                
                
            }

            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExists(Produto.IdProduto))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Listar");
        }

        private bool ProdutoExists(int id)
        {
            return _context.Produto.Any(e => e.IdProduto == id);
        }
    }
}