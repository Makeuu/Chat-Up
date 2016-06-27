using ChatUp.Dal;
using ChatUp.Models;
using ChatUp.ViewModels;
using System;
using System.Collections.Generic;
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
                UtilisateursModels utilisateur = db.ListeUtilisateurs.Find(HttpContext.User.Identity.Name);

                List<GroupeModel> listeGroupes = ListerGroupes(utilisateur);

                listeGroupes = TrierGroupeParDate(listeGroupes);

                return PartialView(listeGroupes);
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
                UtilisateursModels utilisateur = db.ListeUtilisateurs.FirstOrDefault(u => u.Email == HttpContext.User.Identity.Name);
                List<GroupeModel> listeGrp = db.ListeGroupes.ToList();

                foreach (GroupeModel gm in listeGrp)
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
                return View();
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
                    groupeModel.MembresGroupe = new List<UtilisateursModels>();

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
        public ActionResult Editer([Bind(Include = "IdGroupe,NomGroupe,ImageGroupe,InvitationAutorisee,DateCreationGroupe,AdministrateurGroupeId,AdministrateurGroupe,MembresGroupe,ListeMessages")] GroupeModel groupeModel)
        {
            GroupeModel groupe = db.ListeGroupes.Find(groupeModel.IdGroupe);

            groupe.NomGroupe = groupeModel.NomGroupe;
            groupe.InvitationAutorisee = groupeModel.InvitationAutorisee;
            groupe.ImageGroupe = groupeModel.ImageGroupe;
            groupe.AdministrateurGroupe = db.ListeUtilisateurs.Find(groupeModel.AdministrateurGroupeId);

            db.SaveChanges();

            return View("Index");
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
        public ActionResult AfficherMessages(GroupeModel groupe)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
                return PartialView("AfficherMessages", groupe);
            else
                return Redirect("~/Home/Index");
        }

        [ChildActionOnly]
        public ActionResult Informations(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            GroupeModel groupeModel = db.ListeGroupes.Find(id);

            if (groupeModel == null)
                return HttpNotFound();

            return PartialView(groupeModel);
        }

        [HttpGet]
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

        [ChildActionOnly]
        public ActionResult ListerMembres(int? id)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
                return Redirect("~/Home/Index");

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            GroupeModel groupe = db.ListeGroupes.Find(id);

            return PartialView(groupe);
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
                UtilisateursModels utilisateur = db.ListeUtilisateurs.Find(idUtilisateur);

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

                    groupe.ListeMessages.Add(message);
                    discussion.Groupe = groupe;
                    discussion.Message = new MessageModel();

                    db.SaveChanges();
                }

                return PartialView(discussion);
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

        private List<GroupeModel> ListerGroupes(UtilisateursModels utilisateur)
        {
            List<GroupeModel> listeGroupes = db.ListeGroupes.ToList();
            List<GroupeModel> groupeUtilisateur = new List<GroupeModel>();

            foreach (GroupeModel gm in listeGroupes)
            {
                if (gm.MembresGroupe.Contains(utilisateur) || gm.AdministrateurGroupe == utilisateur)
                    groupeUtilisateur.Add(gm);
            }

            return groupeUtilisateur;
        }

        private List<MessageModel> TrierMessageParDate(List<MessageModel> liste)
        {
            if (liste.Count > 0)
            {
                liste = (from msg in liste
                         orderby msg.DateEnvoi descending
                         select msg).ToList();
            }

            return liste;
        }

        private List<GroupeModel> TrierGroupeParDate(List<GroupeModel> listeGrp)
        {
            if (listeGrp.Count > 0)
            {
                listeGrp = (from grp in listeGrp
                            orderby grp.DateCreationGroupe descending
                            select grp).ToList();
            }

            return listeGrp;
        }
    }
}