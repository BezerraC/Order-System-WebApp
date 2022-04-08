using System;
using System.IO;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Microsoft.AspNetCore.Hosting;
using SixLabors.ImageSharp.Formats.Jpeg;
using Microsoft.AspNetCore.Http;

namespace ArkdBarV1
{
    public static class AppUtils
    {
        public static async Task ProcessarArquivoDeImagem(int idProduto, IFormFile imagemProduto, IWebHostEnvironment whe)
        {
            //copia a imagem para um stream me memória
            var ms = new MemoryStream();
            await imagemProduto.CopyToAsync(ms);

            //carrega o stream em memória para o objeto de processamento de imagem
            ms.Position = 0;
            var img = await Image.LoadAsync(ms);
            JpegEncoder jpegEnc = new JpegEncoder();
            jpegEnc.Quality = 100;
            img.SaveAsJpeg(ms, jpegEnc);
            ms.Position = 0;
            img = await Image.LoadAsync(ms);
            ms.Close();
            ms.Dispose();

            //cria um retangulo de recorte para deixar a imagem quadrada
            var tamanho = img.Size();
            Rectangle retanguloCorte;
            if (tamanho.Width > tamanho.Height)
            {
                float x = (tamanho.Width - tamanho.Height) / 2.0F;
                retanguloCorte = new Rectangle((int)x, 0, tamanho.Height, tamanho.Height);
            }
            else
            {
                float y = (tamanho.Height - tamanho.Width) / 2.0F;
                retanguloCorte = new Rectangle(0, (int)y, tamanho.Width, tamanho.Width);
            }
            //recorta a imagem usando o retângulo computado
            img.Mutate(i => i.Crop(retanguloCorte));
            //monta o caminho da imagem (~/img/produto/000000.jpg)
            var caminhoArquivoImagem = Path.Combine(whe.WebRootPath,
                "img//produto", idProduto.ToString("D6") + ".jpeg");
            //cria um arquivo de imagem sobescrevendo o existente, caso exista
            await img.SaveAsync(caminhoArquivoImagem);
        }
    }
}
