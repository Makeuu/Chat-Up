using ChatUp.Dal;
using ChatUp.Models;
using ChatUp.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ChatUp.Controllers
{
    public class DiscussionController : Controller
    {
        private BddContext db = new BddContext();
        
        public ActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
                return View();
            else
                return Redirect("~/Home/Index");
        }

        [ChildActionOnly]
        public ActionResult Lister()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                UtilisateurModel utilisateur = db.ListeUtilisateurs.Find(HttpContext.User.Identity.Name);

                List<GroupeModel> listeGroupes = db.ListeGroupes.ToList();
                List<GroupeModel> groupeUtilisateur = new List<GroupeModel>();

                foreach(GroupeModel gm in listeGroupes)
                {
                    if (gm.MembresGroupe.Contains(utilisateur) || gm.AdministrateurGroupe == utilisateur)
                        groupeUtilisateur.Add(gm);
                }

                return PartialView(groupeUtilisateur);
            }
            else
            {
                return Redirect("~/Home/Index");
            }
        }
        
        public ActionResult Details(int? id)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
                return Redirect("~/Home/Index");
            
            if (id == null)
            {
                UtilisateurModel utilisateur = db.ListeUtilisateurs.FirstOrDefault(u => u.Email == HttpContext.User.Identity.Name);
                List<GroupeModel> listeGrp = db.ListeGroupes.ToList();

                foreach(GroupeModel gm in listeGrp)
                {
                    if (gm.MembresGroupe.Contains(utilisateur) || gm.AdministrateurGroupe == utilisateur)
                    {
                        id = gm.IdGroupe;
                        break;
                    }
                }                
            }

            GroupeModel groupeModel = db.ListeGroupes.Find(id);

            if (groupeModel == null)
                return HttpNotFound();

            return View(groupeModel);
        }
        
        public ActionResult Creer()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                //string idUtilisateur = HttpContext.User.Identity.Name;

                //var listeGroupes = db.ListeGroupes.Where(g => g.AdministrateurGroupeId == HttpContext.User.Identity.Name);

                return View();
            }
            else
                return Redirect("~/Home/Index");
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Creer([Bind(Include = "IdGroupe,NomGroupe,ImageGroupe,InvitationAutorisee,DateCreationGroupe,AdministrateurGroupeId")] GroupeModel groupeModel)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    groupeModel.AdministrateurGroupeId = HttpContext.User.Identity.Name;
                    groupeModel.DateCreationGroupe = DateTime.Now;
                    groupeModel.InvitationAutorisee = false;
                    groupeModel.ListeMessages = new List<MessageModel>();
                    groupeModel.MembresGroupe = new List<UtilisateurModel>();

                    db.ListeGroupes.Add(groupeModel);

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                    return View(groupeModel);
            }
            else
                return Redirect("~/Home/Index");
        }

        public ActionResult Editer(int? id)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
                return Redirect("~/Home/Index");

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            GroupeModel groupeModel = db.ListeGroupes.Find(id);

            if (groupeModel == null)
                return HttpNotFound();

            return View(groupeModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editer([Bind(Include = "IdGroupe,NomGroupe,ImageGroupe,InvitationAutorisee,AdministrateurGroupeId")] GroupeModel groupeModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(groupeModel).State = EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(groupeModel);
        }
        
        public ActionResult Supprimer(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            GroupeModel groupeModel = db.ListeGroupes.Find(id);

            if (groupeModel == null)
                return HttpNotFound();

            return View(groupeModel);
        }
        
        [HttpPost, ActionName("Supprimer")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmerSuppression(int id)
        {
            GroupeModel groupeModel = db.ListeGroupes.Find(id);
            db.ListeGroupes.Remove(groupeModel);

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [ChildActionOnly]
        public ActionResult AfficherMessages(DiscussionViewModel discussion)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
                return PartialView("AfficherMessages", discussion);
            else
                return Redirect("~/Home/Index");
        }

        [ChildActionOnly]
        public ActionResult EnvoyerMessage(int? idGroupe)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                GroupeModel groupe = db.ListeGroupes.Find(idGroupe);

                DiscussionViewModel discussion = new DiscussionViewModel
                {
                    Groupe = groupe
                };

                return PartialView("EnvoyerMessage", discussion);
            }
            else
                return Redirect("~/Home/Index");
        }

        [HttpPost]
        [ChildActionOnly]
        [ValidateAntiForgeryToken]
        public ActionResult EnvoyerMessage(DiscussionViewModel discussion)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                string idUtilisateur = HttpContext.User.Identity.Name;
                int idGroupe = -1;
                UtilisateurModel utilisateur = db.ListeUtilisateurs.Find(idUtilisateur);

                int.TryParse(HttpContext.Request.Url.Segments[HttpContext.Request.Url.Segments.Length - 1].ToString(), out idGroupe);

                if (idGroupe > 0)
                {
                    GroupeModel groupe = db.ListeGroupes.First(g => g.IdGroupe == idGroupe);

                    MessageModel message = new MessageModel
                    {
                        AuteurContenu = utilisateur,
                        DateEnvoi = DateTime.Now,
                        GroupeId = groupe.IdGroupe,
                        Message = discussion.Message.Message
                    };

                    db.ListeMessages.Add(message);
                    groupe.ListeMessages.Add(message);
                    discussion.Groupe = groupe;

                    db.SaveChanges();
                }

                return PartialView();
            }
            else
                return Redirect("~/Home/Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}