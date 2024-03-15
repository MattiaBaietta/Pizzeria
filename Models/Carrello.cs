using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pizzeria.Models
{
    public class Carrello
    {
        public string NomeArt { get; set; }
        public int PrezzoArt { get; set; }
        public int IdPizza { get; set; }
        public int IdArticolo { get; set; }
        public decimal PrezzoIng { get; set; }
        public string NomeIng { get; set; }
        public int TempoTot { get; set; }
        public int? IdOrdine { get; set; }






    }
}