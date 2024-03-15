using Microsoft.Ajax.Utilities;
using Pizzeria.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Deployment.Internal;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pizzeria2.Controllers
{
    public class AdminController : Controller
    {
        DBContext db = new DBContext();

        // GET: Admin
        public ActionResult Index()
        {
            List<Ordine> ordini = db.Ordine.Where(x => x.Stato != "Evaso").ToList();
            List<Carrello> Cart = new List<Carrello>();
            foreach (var o in ordini)
            {

                var joinTabs = from ArticoliIngredienti in db.ArticoliIngredienti
                               join Articoli in db.Articoli on ArticoliIngredienti.IdArticolo equals Articoli.IdArticolo
                               join Ingredienti in db.Ingredienti on ArticoliIngredienti.IdIngrediente equals Ingredienti.IdIngrediente into IngredientiGroup
                               from Ingredienti in IngredientiGroup.DefaultIfEmpty()
                               where ArticoliIngredienti.IdOrdine == o.IdOrdine
                               select new Carrello
                               {
                                   NomeArt = Articoli.NomeArt,
                                   PrezzoArt = Articoli.PrezzoArt,
                                   IdPizza = ArticoliIngredienti.IdPizza,
                                   IdArticolo = Articoli.IdArticolo,
                                   PrezzoIng = Ingredienti != null ? Ingredienti.PrezzoIng : 0,
                                   NomeIng = Ingredienti != null ? Ingredienti.NomeIng : null,
                                   TempoTot = Articoli.TempoArt,
                                   IdOrdine = ArticoliIngredienti.IdOrdine
                               };


                foreach (var item in joinTabs)
                {
                    Cart.Add(item);
                }


            }
            ViewBag.PizzeOrdinate = Cart;
            return View();
        }
        public ActionResult ConfirmOrder(int Id, string Stato)
        {
            var changeorder = db.Ordine.Where(x => x.IdOrdine == Id).ToList();
            List<Ordine> order = changeorder;
            foreach (Ordine item in order)
            {
                item.Stato = Stato;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult AddArt(Articoli a)
        {
            Articoli art = new Articoli
            {
                NomeArt = a.NomeArt,
                FotoArt = a.FotoArt,
                PrezzoArt = a.PrezzoArt,
                TempoArt = a.TempoArt,
            };
            db.Articoli.Add(art);
            db.SaveChanges();
            return View();
        }

        public ActionResult AddArt()
        {
            return View();
        }
        public ActionResult Stats()
        {

            return View();
        }

        public JsonResult TotaleOrdini()
        {
            var results = db.Ordine.Where(x => x.Stato == "Evaso").Count();
            return Json(results, JsonRequestBehavior.AllowGet);
        }
        public JsonResult TotaleData(DateTime d)
        {
            var results = db.Ordine.Where(x => x.Data == d && x.Totale != null).ToList();
            decimal tot = 0;
            foreach (var item in results)
            {
                tot += item.Totale;
            }
            return Json(tot, JsonRequestBehavior.AllowGet);
        }
    }
}