using Pizzeria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Pizzeria.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth
        DBContext db = new DBContext();

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Utenti user, string Passw)
        {
            var loggedUser = db.Utenti.Where(u => u.Username == user.Username && u.Password == user.Password).FirstOrDefault();
            if (loggedUser == null)
            {
                if (user.Password == Passw)
                {
                    db.Utenti.Add(user);
                    db.SaveChanges();
                    FormsAuthentication.SetAuthCookie(user.Username.ToString(), true);
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    TempData["ErrorRegister"] = true;
                    return RedirectToAction("Register");
                }
            }
            else
            {
                TempData["ErrorUser"] = true;
                return RedirectToAction("Register");
            }


        }
        [HttpPost]
        public ActionResult Login(Utenti user)
        {
            var loggedUser = db.Utenti.Where(u => u.Username == user.Username && u.Password == user.Password).FirstOrDefault();
            if (loggedUser == null)
            {
                TempData["ErrorLogin"] = true;
                return RedirectToAction("Login");
            }



            FormsAuthentication.SetAuthCookie(loggedUser.Username.ToString(), true);
            return RedirectToAction("Index", "Home");

        }
    }
}