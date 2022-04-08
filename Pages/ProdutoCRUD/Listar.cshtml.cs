using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ArkdBarV1.Data;
using ArkdBarV1.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ArkdBarV1.Pages.ProdutoCRUD
{
    public class ListarModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ListarModel(ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IList<Produto> Produto { get; set; }

        public async Task OnGetAsync()
        {
            Produto = await _context.Produto.ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produto.FindAsync(id);

            if (produto != null)
            {
                _context.Produto.Remove(produto);
                if (await _context.SaveChangesAsync() > 0)
                {
                    var caminhoArquivoImagem = Path.Combine(
                        _webHostEnvironment.WebRootPath,
                        "img//produto",
                        produto.IdProduto.ToString("D6") + ".jpeg");

                    if (System.IO.File.Exists(caminhoArquivoImagem))
                    {
                        System.IO.File.Delete(caminhoArquivoImagem);
                    }
                }

            }

            return RedirectToPage("./Listar");
        }
    }
}