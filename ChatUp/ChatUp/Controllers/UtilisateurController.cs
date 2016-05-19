using ChatUp.Dal;
using ChatUp.Models;
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
        public ActionResult Inscrire(UtilisateurModel utilisateur)
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