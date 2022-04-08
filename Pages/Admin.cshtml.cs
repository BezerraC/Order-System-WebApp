using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArkdBarV1.Data;
using ArkdBarV1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ArkdBarV1.Pages
{
    public class AdminModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AdminModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Pedido> Pedidos { get; set; }

        public async Task OnGetAsync()
        {
            Pedidos = await _context.Pedidos.Include("Cliente")
                .Where(p => p.Situacao == Pedido.SituacaoPedido.Realizado)
                .OrderByDescending(p => p.DataHoraPedido).ToListAsync();
        }

        public async Task<IActionResult> OnPostCancelarPedidoAsync(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                   .Where(p => p.IdPedido == id)
                   .FirstOrDefaultAsync();

            if (pedido != null)
            {
                pedido.Situacao = Pedido.SituacaoPedido.Cancelado;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Admin");
        }
    }
}