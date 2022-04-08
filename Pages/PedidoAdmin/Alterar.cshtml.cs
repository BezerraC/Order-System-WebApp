using System.Linq;
using System.Threading.Tasks;
using ArkdBarV1.Data;
using ArkdBarV1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ArkdBarV1.Pages.PedidoAdmin
{
    public class AlterarModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public Pedido Pedido { get; set; }

        public AlterarModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            Pedido = await _context.Pedidos.Include("Cliente").FirstOrDefaultAsync(m => m.IdPedido == id);

            if (Pedido == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var situacaoPedido = Pedido.Situacao;
            var enderecoPedido = Pedido.Endereco;
            Pedido = _context.Pedidos.FirstOrDefault(p => p.IdPedido == Pedido.IdPedido);
            Pedido.Situacao = situacaoPedido;
            Pedido.Endereco = enderecoPedido;
            _context.Attach(Pedido).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExistePedido(Pedido.IdPedido))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            Pedido = await _context.Pedidos.Include("Cliente").FirstOrDefaultAsync(m => m.IdPedido == Pedido.IdPedido);
            return RedirectToPage("./Listar");
        }

        private bool ExistePedido(int id)
        {
            return _context.Pedidos.Any(e => e.IdPedido == id);
        }
    }
}