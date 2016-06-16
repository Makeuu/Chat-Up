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
            /* L'argument authentifie est présent dans chaque page pour un contenu intéractif.
             * A voir si on peut le mettre directement dans les pages plutôt que dans chaque vue...
             * Au début je testais la session mais ici la vue travaille avec le model Utilisateur. Du coup la page plante lors du test.
             */
            ViewData["Succes"] = true;
            ViewData["Authentifie"] = HttpContext.User.Identity.IsAuthenticated;
            return View();
        }

        [HttpPost]
        public ActionResult Inscrire(UtilisateurModel utilisateur)
        {
            bool flag = false;
            /*On test si le formulaire est bien rempli*/
            if (ModelState.IsValid)
            {
                DalUtilisateur Dal = new DalUtilisateur();
                flag = Dal.CreerUtilisateur(utilisateur.Email, utilisateur.MotDePasse);
                /*Ajouter une vue d'inscription réussis*/
                /*On test si l'inscription de notre point de vue s'est bien déroulée*/
                if (flag)
                {
                    return RedirectToAction("Index", "Home");
                }
                /*Sinon on renvoit la page avec un message d'erreur*/
                else
                {
                    ViewData["Authentifie"] = HttpContext.User.Identity.IsAuthenticated;
                    ViewData["Succes"] = flag;
                    return View();
                }
            }
            // Sinon on renvoit la page
            ViewData["Authentifie"] = HttpContext.User.Identity.IsAuthenticated;
            ViewData["Succes"] = flag;
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
            ViewData["Authentifie"] = HttpContext.User.Identity.IsAuthenticated;
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
            ViewData["Authentifie"] = HttpContext.User.Identity.IsAuthenticated;
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

        //public ActionResult Groupes()
        //{
        //    ViewData["Authentifie"] = HttpContext.User.Identity.IsAuthenticated;
        //    return View();
        //}

        public ActionResult Profil()
        {
            ViewData["Authentifie"] = HttpContext.User.Identity.IsAuthenticated;
            return View();
        }

    }
}
