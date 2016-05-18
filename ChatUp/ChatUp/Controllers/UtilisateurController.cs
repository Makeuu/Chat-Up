using ChatUp.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChatUp.Controllers
{
    public class UtilisateurController : Controller
    {
        // GET: Utilisateur
        public ActionResult Login()
        {
            return View();
        }

        // GET : Formulaire d'inscription
        public ActionResult Inscrire()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Inscrire(UtilisateurModels utilisateur)
        {
            if (ModelState.IsValid)
            {
                DalUtilisateur Dal = new DalUtilisateur();
                Dal.CreerUtilisateur(utilisateur.Email, utilisateur.MotDePasse);
                return RedirectToAction("Index", "Home");
            }


            // If we got this far, something failed, redisplay form
            return View();
        }
    }
}