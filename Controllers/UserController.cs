using Microsoft.Ajax.Utilities;
using Pizzeria.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;

namespace Pizzeria2.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        DBContext db = new DBContext();



        public ActionResult Order()
        {

            List<Articoli> list = db.Articoli.ToList();
            List<Ingredienti> ingredients = db.Ingredienti.ToList();
            ViewBag.id = Session["Pizza"];
            ViewBag.ingid = Session["Ingredienti"];

            ViewBag.Ingredienti = ingredients;
            ViewBag.Pizze = list;
            return View();

        }


        public ActionResult ViewCart()
        {
            string NUtente = User.Identity.Name;
            var joinTabs = from ArticoliIngredienti in db.ArticoliIngredienti
                           join Articoli in db.Articoli on ArticoliIngredienti.IdArticolo equals Articoli.IdArticolo
                           join Ingredienti in db.Ingredienti on ArticoliIngredienti.IdIngrediente equals Ingredienti.IdIngrediente
                           where ArticoliIngredienti.Username == NUtente && ArticoliIngredienti.IdOrdine == null
                           select new Carrello
                           {
                               NomeArt = Articoli.NomeArt,
                               PrezzoArt = Articoli.PrezzoArt,
                               IdPizza = ArticoliIngredienti.IdPizza,
                               IdArticolo = Articoli.IdArticolo,
                               PrezzoIng = Ingredienti.PrezzoIng,
                               NomeIng = Ingredienti.NomeIng,
                               TempoTot = Articoli.TempoArt,

                           };
            List<Carrello> Cart = joinTabs.ToList();
            ViewBag.ListaPizze = Cart;
            decimal totale = 0;
            int? oldid = null;
            int tempotot = 0;
            foreach (Carrello c in Cart)
            {
                if (c.IdPizza == oldid)
                {
                    totale += c.PrezzoIng;

                }
                else
                {
                    oldid = c.IdPizza;
                    totale += c.PrezzoIng + c.PrezzoArt;
                    tempotot = c.TempoTot;
                }

            }
            Session["Totale"] = totale;
            Session["ListaPizze"] = Cart;
            Session["Tempo"] = tempotot;
            ViewBag.totale = totale;
            return View();
        }

        public ActionResult Confirm(string Indirizzo, string Note)
        {
            DateTime now = DateTime.Now;
            if (Indirizzo == "")
            {
                TempData["ErrorIndirizzo"] = true;
                return RedirectToAction("ViewCart");
            }
            int scopeordine = 0;
            var addorder = new Ordine
            {
                Totale = (decimal)Session["Totale"],
                Indirizzo = Indirizzo,
                Note = Note,
                Stato = "Ricevuto",
                Username = User.Identity.Name,
                Data = now,
            };

            db.Ordine.Add(addorder);

            db.SaveChanges();



            scopeordine = db.Ordine.Max(x => x.IdOrdine);



            var updateorder = db.ArticoliIngredienti.Where(x => x.IdOrdine == null);

            foreach (var item in updateorder)
            {
                item.IdOrdine = scopeordine;


            }
            db.SaveChanges();

            return RedirectToAction("ExtimatedTime");
        }

        public ActionResult ExtimatedTime()
        {
            DateTime now = DateTime.Now;
            DateTime nuovoOrario = now.AddMinutes((int)Session["Tempo"]);

            nuovoOrario = nuovoOrario.AddSeconds(-nuovoOrario.Second);

            string orarioFormattato = nuovoOrario.ToString("HH:mm");

            ViewBag.Tempo = orarioFormattato;
            return View();
        }

        public ActionResult AddIng(int IdArticolo, int IdIngrediente)
        {
            if (Session["Pizza"] != null && (int)Session["Pizza"] != IdArticolo)
            {
                Session["Ingredienti"] = null;
            }

            List<Ingredienti> temping = Session["Ingredienti"] as List<Ingredienti> ?? new List<Ingredienti>();
            string Username = User.Identity.Name;
            Ingredienti i = db.Ingredienti.Where(u => u.IdIngrediente == IdIngrediente).FirstOrDefault();
            temping.Add(i);
            Session["Ingredienti"] = temping;
            Session["Pizza"] = IdArticolo;
            return RedirectToAction("Order");
        }
        public ActionResult AddArt(int IdArticolo)
        {
            string NUtente = User.Identity.Name;
            List<Ingredienti> ingredienti = Session["Ingredienti"] as List<Ingredienti>;
            int idpizza = db.ArticoliIngredienti.OrderByDescending(x => x.IdPizza).Select(x => x.IdPizza).FirstOrDefault();
            idpizza++;
            if (ingredienti != null)
            {
                foreach (var a in ingredienti)
                {
                    ArticoliIngredienti PizIng = new ArticoliIngredienti
                    {
                        Username = NUtente,
                        IdArticolo = IdArticolo,
                        IdIngrediente = a.IdIngrediente,
                        IdPizza = idpizza
                    };
                    db.ArticoliIngredienti.Add(PizIng);
                    db.SaveChanges();
                }
            }
            else
            {
                ArticoliIngredienti PizIng = new ArticoliIngredienti
                {
                    Username = NUtente,
                    IdArticolo = IdArticolo,
                    IdPizza = idpizza
                };
                db.ArticoliIngredienti.Add(PizIng);
                db.SaveChanges();
            }
            Session["Ingredienti"] = null;
            Session["Pizza"] = null;
            return RedirectToAction("Order");
        }
        public ActionResult RemoveP(Carrello c)
        {
            var query = db.ArticoliIngredienti.Where(x => x.IdPizza == c.IdPizza).ToList();
            db.ArticoliIngredienti.RemoveRange(query);
            db.SaveChanges();
            return RedirectToAction("ViewCart");
        }

    }
}