namespace Pizzeria.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ingredienti")]
    public partial class Ingredienti
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ingredienti()
        {
            ArticoliIngredienti = new HashSet<ArticoliIngredienti>();
        }

        [Key]
        public int IdIngrediente { get; set; }

        [Required]
        public string NomeIng { get; set; }

        public decimal PrezzoIng { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ArticoliIngredienti> ArticoliIngredienti { get; set; }
    }
}
