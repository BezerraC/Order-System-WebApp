using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArkdBarV1.Data;
using ArkdBarV1.Models;
using ArkdBarV1.Services;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ArkdBarV1.Pages
{
    [Authorize(Roles = "cliente")]
    public class FinalizarPedidoModel : PageModel
    {
        private ApplicationDbContext _context;

        public string COOKIE_NAME
        {
            get { return ".AspNetCore.CartId"; }
        }


        public Pedido Pedido { get; set; }

        public Cliente Cliente { get; set; }

        public FinalizarPedidoModel(ApplicationDbContext context)
        {
            _context = context;
        }


        static BaseFont fonteBase = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

        public async Task<IActionResult> OnGetAsync()
        {
            if (Request.Cookies.ContainsKey(COOKIE_NAME))
            {
                var cartId = Request.Cookies[COOKIE_NAME];

                Pedido = await _context.Pedidos.Include("ItensPedido").
                    Include("ItensPedido.Produto").FirstOrDefaultAsync(p => p.IdCarrinho == cartId);

                Cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Email == User.Identity.Name);

                
               
                if (Pedido.IdCliente > 0)
                {
                    Pedido.Situacao = Pedido.SituacaoPedido.Realizado;
                    Pedido.DataHoraPedido = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                    Response.Cookies.Delete(COOKIE_NAME);
                    await GerarRelatorioEmPDF();
                    return Page();
                }
                else
                {
                    return RedirectToPage("/ConfirmarPedido");
                }
            }

            return RedirectToPage("/Carrinho");
        }


        
        private async Task GerarRelatorioEmPDF()
        {

            //configuração de documento PDF
            var pxPorMm = 72 / 25.5F;
            var pdf = new Document(PageSize.A5, 15 * pxPorMm, 15 * pxPorMm,
                15 * pxPorMm, 20 * pxPorMm);

            var nomeArquivo = $"pedidos.{DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss")}.pdf";
            var arquivo = new FileStream(nomeArquivo, FileMode.Create);
            var writer = PdfWriter.GetInstance(pdf, arquivo);
            writer.PageEvent = new EventosDePagina();
            pdf.Open();

          
            //Adição do título
            var fonteParagrafo = new iTextSharp.text.Font(fonteBase, 28,
                iTextSharp.text.Font.NORMAL, BaseColor.Black);
            var titulo = new Paragraph($"Pedido: {Pedido.IdPedido.ToString("D6")}", fonteParagrafo);
            titulo.Alignment = Element.ALIGN_CENTER;
            titulo.SpacingAfter = 10;
            pdf.Add(titulo);


            //Informa a atendente
            var fonteUser = new iTextSharp.text.Font(fonteBase, 16,
                iTextSharp.text.Font.NORMAL, BaseColor.Black);
            var atendente = new Paragraph($"Atendente: {Cliente.Nome.Substring(0, Cliente.Nome.IndexOf(' '))}", fonteUser);
            titulo.Alignment = Element.ALIGN_LEFT;
            pdf.Add(atendente);


            //Adição da tabela de dados
            var tabela = new PdfPTable(4);
            float[] larguraColunas = { 2f, 0.5f, 0.5f, 0.5f };
            tabela.SetWidths(larguraColunas);
            tabela.DefaultCell.BorderWidth = 0;
            tabela.WidthPercentage = 100;


            //Adição das Células de títulos das colunas
            CriarCelulaTexto(tabela, "Item", PdfPCell.ALIGN_LEFT, true);
            CriarCelulaTexto(tabela, "Qtde.", PdfPCell.ALIGN_CENTER, true);
            CriarCelulaTexto(tabela, "R$ Unit.", PdfPCell.ALIGN_CENTER, true);
            CriarCelulaTexto(tabela, "R$ Total.", PdfPCell.ALIGN_CENTER, true);

            foreach (var item in Pedido.ItensPedido)
            {
                CriarCelulaTexto(tabela, $"{item.Produto.Nome}",PdfPCell.ALIGN_LEFT);
                CriarCelulaTexto(tabela, $"{item.Quantidade}", PdfPCell.ALIGN_CENTER);
                CriarCelulaTexto(tabela, $"{item.ValorUnitario.ToString("F2")}", PdfPCell.ALIGN_CENTER);
                CriarCelulaTexto(tabela, $"{item.ValorItem.ToString("F2")}", PdfPCell.ALIGN_CENTER);
            }
            pdf.Add(tabela);


            //Informa o cliente
            var fonteCliente = new iTextSharp.text.Font(fonteBase, 16,
                iTextSharp.text.Font.NORMAL, BaseColor.Black);
            var cliente = new Paragraph($"Cliente: XXX", fonteCliente);
            titulo.Alignment = Element.ALIGN_LEFT;
            pdf.Add(cliente);


            //Informa o número da mesa
            var fonteMesa = new iTextSharp.text.Font(fonteBase, 16,
                iTextSharp.text.Font.NORMAL, BaseColor.Black);
            var mesa = new Paragraph($"Mesa: XXX", fonteMesa);
            titulo.Alignment = Element.ALIGN_LEFT;
            pdf.Add(mesa);


            //Informa o número da mesa
            var fonteObs = new iTextSharp.text.Font(fonteBase, 16,
                iTextSharp.text.Font.NORMAL, BaseColor.Black);
            var obs = new Paragraph($"Obs:.", fonteObs);
            titulo.Alignment = Element.ALIGN_LEFT;
            pdf.Add(obs);


            //Informa o valor total
            var fonteFooter = new iTextSharp.text.Font(fonteBase, 16,
                iTextSharp.text.Font.NORMAL, BaseColor.Black);
            var valorTotal = new Paragraph($"Valor Total: R$ {Pedido.ValorTotal.ToString("F2")}", fonteFooter);
            titulo.Alignment = Element.ALIGN_LEFT;
            pdf.Add(valorTotal);


            pdf.Close();
            arquivo.Close();

            //abre o PDF no visualizador padrão
            var caminhoPDF = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nomeArquivo);
            Process.Start(new ProcessStartInfo()
            {
                //Para Mac OS
                Arguments = $"~/ start {caminhoPDF}",
                FileName = "bin/bash",
                UseShellExecute = true


                //Para Windows

                //Arguments = $"/c start {caminhoPDF}",
                //FileName = "cmd.exe",
                //CreateNoWindow = true

            });
        }

        static void CriarCelulaTexto(PdfPTable tabela, string texto,
            int alinhamentoHorz = PdfPCell.ALIGN_LEFT,
            bool negrito = false, bool italico = false,
            int tamanhoFonte = 12, int alturaCelula = 25)
        {
            int estilo = iTextSharp.text.Font.NORMAL;
            if (negrito && italico)
            {
                estilo = iTextSharp.text.Font.BOLDITALIC;
            }
            else if (negrito)
            {
                estilo = iTextSharp.text.Font.BOLD;
            }
            else if (italico)
            {
                estilo = iTextSharp.text.Font.ITALIC;
            }
            var fonteCelula = new iTextSharp.text.Font(fonteBase, tamanhoFonte,
                estilo, BaseColor.Black);
            var celula = new PdfPCell(new Phrase(texto, fonteCelula));
            celula.HorizontalAlignment = alinhamentoHorz;
            celula.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            celula.Border = 0;
            celula.BorderWidthBottom = 1;
            celula.FixedHeight = alturaCelula;
            tabela.AddCell(celula);
        }
    }
}