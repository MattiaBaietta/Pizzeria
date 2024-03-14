namespace Pizzeria.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ordine")]
    public partial class Ordine
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ordine()
        {
            ArticoliIngredienti = new HashSet<ArticoliIngredienti>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdOrdine { get; set; }

        public int Totale { get; set; }

        [Required]
        public string Indirizzo { get; set; }

        [Required]
        public string Note { get; set; }

        [Required]
        [StringLength(50)]
        public string Stato { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ArticoliIngredienti> ArticoliIngredienti { get; set; }

        public virtual Utenti Utenti { get; set; }
    }
}
