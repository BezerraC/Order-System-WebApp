using System.Linq;
using System.Threading.Tasks;
using ArkdBarV1.Data;
using ArkdBarV1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ArkdBarV1.Pages.Admin.PedidoAdmin
{
    [Authorize(Roles = "admin")]
    public class DetalhesPedidoModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetalhesPedidoModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Pedido Pedido { get; set; }

        public async Task OnGetAsync([FromQuery] int? id)
        {
            if (id.HasValue)
            {
                Pedido = await _context.Pedidos.Include("Cliente")
                    .Include("ItensPedido").Include("ItensPedido.Produto")
                    .Where(p => p.IdPedido == id)
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<IActionResult> OnPostAtenderPedidoAsync(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            Pedido = await _context.Pedidos
                   .Where(p => p.IdPedido == id)
                   .FirstOrDefaultAsync();

            if (Pedido != null)
            {
                Pedido.Situacao = Pedido.SituacaoPedido.Atendido;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/PedidoAdmin/Listar");
        }
    }
}
