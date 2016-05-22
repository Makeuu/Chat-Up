using ChatUp.Dal;
using ChatUp.Models;
using System;
using System.Web.Mvc;

namespace ChatUp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            SessionModel viewModel = new SessionModel { Authentifie = HttpContext.User.Identity.IsAuthenticated };
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                DalUtilisateur Dal = new DalUtilisateur();
                viewModel.Utilisateur = Dal.ObtenirUtilisateur(HttpContext.User.Identity.Name);
                ViewData["Pseudo"] = viewModel.Utilisateur.Email;

            }
            ViewData["Authentifie"] = HttpContext.User.Identity.IsAuthenticated;
            return View(viewModel);
        }
    }
}