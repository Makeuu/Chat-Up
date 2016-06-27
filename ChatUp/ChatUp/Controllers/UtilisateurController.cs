using ChatUp.Dal;
using ChatUp.Models;
using System;
using System.Linq;
using System.Web;
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

        public ActionResult AjouterAmi()
        {
            UtilisateurModel viewModel = db.ListeUtilisateurs.FirstOrDefault(u => u.Email == HttpContext.User.Identity.Name);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AjouterAmi(UtilisateurModel viewModel)
        {
            UtilisateurModel amis = db.ListeUtilisateurs.FirstOrDefault(u => u.Email == viewModel.Email);
            UtilisateurModel courant = db.ListeUtilisateurs.FirstOrDefault(u => u.Email == HttpContext.User.Identity.Name);

            if (amis != null && amis.Email != courant.Email)
            {
                courant.ListeAmis.Add(amis);
                db.SaveChanges();
                return RedirectToAction("AjouterAmisSucces");
            }
            else
            {
                return RedirectToAction("AjouterAmisEchec");
            }
        }

        public ActionResult AjouterAmisSucces()
        {
            UtilisateurModel viewModel = db.ListeUtilisateurs.FirstOrDefault(u => u.Email == HttpContext.User.Identity.Name);
            return View(viewModel);
        }

        public ActionResult AjouterAmisEchec()
        {
            UtilisateurModel viewModel = db.ListeUtilisateurs.FirstOrDefault(u => u.Email == HttpContext.User.Identity.Name);
            return View(viewModel);
        }

        public ActionResult ListeAmis()
        {
            UtilisateurModel viewModel = db.ListeUtilisateurs.FirstOrDefault(u => u.Email == HttpContext.User.Identity.Name);
            return PartialView(viewModel);
        }

    }
}
