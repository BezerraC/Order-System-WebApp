using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ArkdBarV1.Data;

namespace ArkdBarV1.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PedidoAPIController : ControllerBase
    {
        private ApplicationDbContext _context;

        public PedidoAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<JsonResult> AtualizarItemPedido([FromForm] int? idPedido,
            [FromForm] int? idProduto = 0, [FromForm] int? quantidade = 0)
        {
            if ((!idPedido.HasValue) || (idProduto <= 0) ||
                (quantidade <= 0)) return new JsonResult(false);

            var pedido = await _context.Pedidos
                .Include("ItensPedido")
                .Include("ItensPedido.Produto")
                .FirstOrDefaultAsync(p => p.IdPedido == idPedido);

            if (pedido != null)
            {
                if (pedido.Situacao != Models.Pedido.SituacaoPedido.Carrinho)
                {
                    var itemPedido = pedido.ItensPedido.FirstOrDefault(ip => ip.IdProduto == idProduto);

                    if (itemPedido != null)
                    {
                        itemPedido.Quantidade = quantidade.Value;

                        if (_context.SaveChanges() > 0)
                        {
                            double valorPedido = pedido.ItensPedido.Sum(ip => ip.ValorItem);
                            var item = pedido.ItensPedido.Select(
                                x => new { id = x.IdProduto, q = x.Quantidade, v = x.ValorItem }).
                                FirstOrDefault(ip => ip.id == idProduto);
                            var jsonRes = new JsonResult(new { v = valorPedido, item });
                            return jsonRes;
                        }

                    }
                }
            }
            return new JsonResult(false);
        }

        [HttpPost]
        public async Task<JsonResult> ExcluirItemPedido([FromForm] int? idPedido,
            [FromForm] int? idProduto = 0)
        {
            if ((!idPedido.HasValue) || (idProduto <= 0)) return new JsonResult(false);

            var pedido = await _context.Pedidos.Include("ItensPedido").
                FirstOrDefaultAsync(p => p.IdPedido == idPedido);

            if (pedido != null)
            {
                if (pedido.Situacao != Models.Pedido.SituacaoPedido.Carrinho)
                {
                    var itemPedido = pedido.ItensPedido.FirstOrDefault(ip => ip.IdProduto == idProduto);
                    if (itemPedido != null)
                    {
                        pedido.ItensPedido.Remove(itemPedido);

                        if (_context.SaveChanges() > 0)
                        {
                            double valorPedido = pedido.ItensPedido.Sum(ip => ip.ValorItem);
                            var jsonRes = new JsonResult(new { v = valorPedido, id = idProduto });
                            return jsonRes;
                        }
                    }
                }
            }
            return new JsonResult(false);
        }
    }
}