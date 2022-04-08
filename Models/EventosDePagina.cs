using System;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ArkdBarV1.Models
{
    public class EventosDePagina : PdfPageEventHelper
    {
        private PdfContentByte wdc;

        private BaseFont fonteBaseRodape { get; set; }

        private iTextSharp.text.Font fonteRodape { get; set; }

        public EventosDePagina()
        {
            fonteBaseRodape = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            fonteRodape = new iTextSharp.text.Font(fonteBaseRodape, 8f,
                iTextSharp.text.Font.NORMAL, BaseColor.Black);
        }

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            base.OnOpenDocument(writer, document);
            this.wdc = writer.DirectContent;
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);
            AdicionarMomentoGeracaoRelatorio(writer, document);
            AdicionarAtt(writer, document);
        }

        private void AdicionarMomentoGeracaoRelatorio(PdfWriter writer, Document document)
        {
            var textoMomentoGeracao = $"Gerado em {DateTime.Now.ToShortDateString()} às " +
                            $"{DateTime.Now.ToShortTimeString()}";

            wdc.BeginText();
            wdc.SetFontAndSize(fonteRodape.BaseFont, fonteRodape.Size);
            wdc.SetTextMatrix(document.LeftMargin,
                document.BottomMargin * 0.75f);
            wdc.ShowText(textoMomentoGeracao);
            wdc.EndText();
        }

        private void AdicionarAtt(PdfWriter writer, Document document)
        {
            var textoAtt = $"Att. Equipe ARKD BAR ";

            float larguraTextoPaginacao =
                fonteBaseRodape.GetWidthPoint(textoAtt, fonteRodape.Size);

            var tamanhoPagina = document.PageSize;

            wdc.BeginText();
            wdc.SetFontAndSize(fonteRodape.BaseFont, fonteRodape.Size);
            wdc.SetTextMatrix(tamanhoPagina.Width - document.RightMargin - larguraTextoPaginacao,
                document.BottomMargin * 0.75f);
            wdc.ShowText(textoAtt);
            wdc.EndText();
        }
    }
} 
