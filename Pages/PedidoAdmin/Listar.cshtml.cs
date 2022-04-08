using ArkdBarV1.Data;
using ArkdBarV1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArkdBarV1.Pages.PedidoAdmin
{
    public class ListarModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ListarModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Pedido> Pedidos { get; set; }

        public async Task OnGetAsync()
        {
            Pedidos = await _context.Pedidos.Include("Cliente")
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

            return RedirectToPage("./Listar");
        }

        public async Task<IActionResult> OnPostExcluirPedidoAsync(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos.FirstOrDefaultAsync(p => p.IdPedido == id);

            if (pedido != null)
            {
                _context.Pedidos.Remove(pedido);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Listar");
        }
    }
}
