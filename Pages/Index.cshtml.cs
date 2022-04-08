using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArkdBarV1.Data;
using ArkdBarV1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ArkdBarV1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private ApplicationDbContext _context;

        public IList<Produto> Produtos;

        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            _logger = logger;

            _context = context;
        }

        public async Task OnGetAsync([FromQuery]string termoBusca)
        {
            if (string.IsNullOrEmpty(termoBusca))
            {
                Produtos = await _context.Produto.ToListAsync<Produto>();
            }
            else
            {
                //filtro de produto
                Produtos = await _context.Produto.Where(
                    p => p.Nome.ToLower().Contains(termoBusca.ToLower())).ToListAsync();
            }

        }
    }
}
