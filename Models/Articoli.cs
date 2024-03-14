namespace Pizzeria.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Articoli")]
    public partial class Articoli
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Articoli()
        {
            ArticoliIngredienti = new HashSet<ArticoliIngredienti>();
        }

        [Key]
        public int IdArticolo { get; set; }

        [Required]
        public string NomeArt { get; set; }

        public string FotoArt { get; set; }

        public int PrezzoArt { get; set; }

        public int TempoArt { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ArticoliIngredienti> ArticoliIngredienti { get; set; }
    }
}
