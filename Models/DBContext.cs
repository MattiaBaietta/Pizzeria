using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Pizzeria.Models
{
    public partial class DBContext : DbContext
    {
        public DBContext()
            : base("name=DBContext")
        {
        }

        public virtual DbSet<Articoli> Articoli { get; set; }
        public virtual DbSet<ArticoliIngredienti> ArticoliIngredienti { get; set; }
        public virtual DbSet<Ingredienti> Ingredienti { get; set; }
        public virtual DbSet<Ordine> Ordine { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Utenti> Utenti { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Articoli>()
                .HasMany(e => e.ArticoliIngredienti)
                .WithRequired(e => e.Articoli)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ArticoliIngredienti>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<Ordine>()
                .Property(e => e.Indirizzo)
                .IsUnicode(false);

            modelBuilder.Entity<Ordine>()
                .Property(e => e.Note)
                .IsUnicode(false);

            modelBuilder.Entity<Ordine>()
                .Property(e => e.Stato)
                .IsUnicode(false);

            modelBuilder.Entity<Ordine>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<Utenti>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<Utenti>()
                .HasMany(e => e.Ordine)
                .WithRequired(e => e.Utenti)
                .WillCascadeOnDelete(false);
        }
    }
}
