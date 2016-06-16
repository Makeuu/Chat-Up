using ChatUp.Dal;
using ChatUp.Models;
using System;
using System.Web.Mvc;
using System.Web.Security;

namespace ChatUp.Controllers
{
    public class UtilisateurController : Controller
    {
        private BddContext db = new BddContext();

        // GET : Formulaire d'inscription
        public ActionResult Inscrire()
        {
 
            ViewData["Succes"] = true;
            return View();
        }

        [HttpPost]
        public ActionResult Inscrire(UtilisateurModel utilisateur)
        {
            //bool flag = false;
            ///*On test si le formulaire est bien rempli*/
            //if (ModelState.IsValid)
            //{
            //    DalUtilisateur Dal = new DalUtilisateur();
            //    flag = Dal.CreerUtilisateur(utilisateur.Email, utilisateur.MotDePasse);
            //    /*Ajouter une vue d'inscription réussis*/
            //    /*On test si l'inscription de notre point de vue s'est bien déroulée*/
            //    if (flag)
            //    {
            //        return RedirectToAction("Index", "Home");
            //    }
            //    /*Sinon on renvoit la page avec un message d'erreur*/
            //    else
            //    {
            //        ViewData["Succes"] = flag;
            //        return View();
            //    }
            //}
            //// Sinon on renvoit la page
            //ViewData["Succes"] = flag;
            //return View();

            if (ModelState.IsValid)
            {
                ProfilModel profil = new ProfilModel
                {
                    Anniversaire = DateTime.Now
                };

                utilisateur.DateInscription = DateTime.Now;
                utilisateur.Profil = profil;

                db.ListeProfils.Add(profil);
                db.ListeUtilisateurs.Add(utilisateur);
                db.SaveChanges();
            }

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
            if (session != null)
            {
                DalS.SupprimerSession(session);
            }

            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}
