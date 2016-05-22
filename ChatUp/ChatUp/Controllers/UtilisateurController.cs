using ChatUp.Dal;
using ChatUp.Models;
using System.Web.Mvc;
using System.Web.Security;

namespace ChatUp.Controllers
{
    public class UtilisateurController : Controller
    {
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

        public ActionResult Login()
        {
            SessionModel viewModel = new SessionModel { Authentifie = HttpContext.User.Identity.IsAuthenticated };
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                DalUtilisateur Dal = new DalUtilisateur();
                viewModel.Utilisateur = Dal.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            }
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Login(SessionModel viewModel)
        {
            if (ModelState.IsValid)
            {
                DalSession Dal = new DalSession();
                //SessionModel session = Dal.AuthentifierUtilisateur(viewModel.Utilisateur.Prenom, viewModel.Utilisateur.MotDePasse);
                SessionModel session = Dal.AuthentifierUtilisateur(viewModel.Email, viewModel.MotDePasse);
                if (session.Utilisateur != null)
                {
                    FormsAuthentication.SetAuthCookie(session.Utilisateur.Email, true);
                    return Redirect("/");
                }
                else
                {
                    Dal.SupprimerSession(session);
                }
                ModelState.AddModelError("Utilisateur.Prenom", "Prénom et/ou mot de passe incorrect(s)");
            }
            return View(viewModel);
        }
        public ActionResult Deconnexion()
        {
            DalSession DalS = new DalSession();
            DalUtilisateur DalU = new DalUtilisateur();

            UtilisateurModel utilisateur = DalU.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            SessionModel session = DalS.ObtenirSession(utilisateur.Email);
            if(session != null)
            {
                DalS.SupprimerSession(session);
            }

            FormsAuthentication.SignOut();
            return Redirect("/");
        }
    }
}
