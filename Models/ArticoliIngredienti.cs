namespace Pizzeria.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ArticoliIngredienti")]
    public partial class ArticoliIngredienti
    {
        [StringLength(50)]
        public string Username { get; set; }

        public int? IdIngrediente { get; set; }

        public int IdArticolo { get; set; }

        public int? IdOrdine { get; set; }

        [Key]
        public int Id { get; set; }

        public int IdPizza { get; set; }

        public virtual Articoli Articoli { get; set; }

        public virtual Ingredienti Ingredienti { get; set; }

        public virtual Ordine Ordine { get; set; }

        public virtual Utenti Utenti { get; set; }
    }
}
