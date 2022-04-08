using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ArkdBarV1.Models;
using System.Threading.Tasks;

namespace ArkdBarV1.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ItemPedido>()
                .HasKey(e => new { e.IdPedido, e.IdProduto });

            //Restringe a exclusão de cliente que possuem pedidos
            modelBuilder.Entity<Pedido>()
                .HasOne<Cliente>(p => p.Cliente)
                .WithMany(c => c.Pedidos)
                .HasForeignKey(p => p.IdCliente)
                .OnDelete(DeleteBehavior.Restrict);

            //Exclui automaticamente os itens de um pedido quando um pedido é excluido
            modelBuilder.Entity<ItemPedido>()
                .HasOne<Pedido>(ip => ip.Pedido)
                .WithMany(p => p.ItensPedido)
                .HasForeignKey(p => p.IdPedido)
                .OnDelete(DeleteBehavior.Cascade);

            //Restringe exclusão de produtos que possuem itens pedidos
            modelBuilder.Entity<ItemPedido>()
                .HasOne<Produto>(ip => ip.Produto)
                .WithMany()
                .HasForeignKey(p => p.IdProduto)
                .OnDelete(DeleteBehavior.Restrict);



        }


        public DbSet<ArkdBarV1.Models.Produto> Produto { get; set; }

        public DbSet<Cliente > Clientes { get; set; }

        public DbSet<ArkdBarV1.Models.Pedido> Pedidos { get; set; }

        public DbSet<ArkdBarV1.Models.ItemPedido> ItensPedido { get; set; }

    }
}
