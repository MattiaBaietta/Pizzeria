using Pizzeria.Models;
using System;
using System.Collections.Generic;
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
                           };
            List<Carrello> Cart = joinTabs.ToList();
            ViewBag.ListaPizze = Cart;
            var variabile = ViewBag.ListaPizze[0].NomeArt;

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

            return RedirectToAction("Order");
        }


    }
}