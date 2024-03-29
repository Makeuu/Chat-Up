﻿using ChatUp.Dal;
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
            {
                UtilisateurModel utilisateur = db.ListeUtilisateurs.FirstOrDefault(u => u.Email == HttpContext.User.Identity.Name);
                List<GroupeModel> listeGrp = db.ListeGroupes.ToList();

                if (listeGrp.Count > 0)
                {
                    int id = 0;

                    foreach (GroupeModel gm in listeGrp)
                    {
                        if (gm.MembresGroupe.Contains(utilisateur) || gm.AdministrateurGroupe == utilisateur)
                        {
                            id = gm.IdGroupe;
                            break;
                        }
                    }

                    GroupeModel groupeModel = db.ListeGroupes.Find(id);

                    if (groupeModel == null)
                        return View();
                    
                    return RedirectToAction("Details", new { id = groupeModel.IdGroupe });
                }
                else
                {
                    return View();
                }
            }
            else
                return Redirect("~/Home/Index");
        }

        [ChildActionOnly]
        public ActionResult Lister()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                UtilisateurModel utilisateur = db.ListeUtilisateurs.Find(HttpContext.User.Identity.Name);

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
                UtilisateurModel utilisateur = db.ListeUtilisateurs.FirstOrDefault(u => u.Email == HttpContext.User.Identity.Name);
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
                    groupeModel.MembresGroupe = new List<UtilisateurModel>();

                    db.ListeGroupes.Add(groupeModel);

                    db.SaveChanges();

                    return RedirectToAction("Details", new { id = groupeModel.IdGroupe });
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

            return RedirectToAction("Details", new { id = groupe.IdGroupe });
        }

        public ActionResult Supprimer(int? id)
        {
            GroupeModel groupeModel = db.ListeGroupes.Find(id);

            groupeModel.MembresGroupe.RemoveRange(0, groupeModel.MembresGroupe.Count);
            db.SaveChanges();

            db.ListeGroupes.Remove(groupeModel);           
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        
        public ActionResult EnvoyerMessagePrive(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            UtilisateurModel ami = db.ListeUtilisateurs.FirstOrDefault(u => u.Profil.ProfilId == id);
            UtilisateurModel util = db.ListeUtilisateurs.Find(HttpContext.User.Identity.Name);
            
            List<GroupeModel> listeGrp = db.ListeGroupes.Where(g => g.AdministrateurGroupe.Email == util.Email).ToList();
            GroupeModel grp = null;

            foreach(GroupeModel g in listeGrp)
            {
                if(g.MembresGroupe.Contains(ami) && g.MembresGroupe.Count == 1)
                {
                    grp = g;
                    break;
                }
            }

            if (grp == null)
            {
                grp = new GroupeModel
                {
                    AdministrateurGroupe = util,
                    DateCreationGroupe = DateTime.Now,
                    ImageGroupe = null,
                    InvitationAutorisee = true,
                    ListeMessages = new List<MessageModel>(),
                    MembresGroupe = new List<UtilisateurModel>(),
                    NomGroupe = ami.Profil.Prenom + " " + ami.Profil.Nom
                };

                grp.MembresGroupe.Add(ami);
                db.ListeGroupes.Add(grp);
            }

            db.SaveChanges();

            return RedirectToAction("Details", new { id = grp.IdGroupe });
        }

        public ActionResult AfficherMessages(int? id)
        {
            if (HttpContext.User.Identity.IsAuthenticated && id != null)
            {
                GroupeModel groupe = db.ListeGroupes.Find(id);

                return PartialView("AfficherMessages", groupe);
            }
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

        public ActionResult InviterMembre(int? idGroupe, string idUtilisateur)
        {
            if (idGroupe == null)
                return Redirect("~/Home/Index.cshtml");

            DiscussionViewModel viewModel;
            GroupeModel groupe = db.ListeGroupes.Find(idGroupe);
            UtilisateurModel utilisateur;

            if (idUtilisateur == null)
                utilisateur = db.ListeUtilisateurs.Find(HttpContext.User.Identity.Name);
            else
                utilisateur = db.ListeUtilisateurs.Find(idUtilisateur);

            if (!groupe.MembresGroupe.Contains(utilisateur) && utilisateur.Email != HttpContext.User.Identity.Name)
            {
                groupe.MembresGroupe.Add(utilisateur);
                db.SaveChanges();

                return RedirectToAction("Details", new { id = groupe.IdGroupe });
            }

            viewModel = new DiscussionViewModel
            {
                Groupe = groupe,
                Utilisateur = utilisateur
            };

            return PartialView(viewModel);
        }

        public ActionResult ExclureMembre(int? idGroupe, string idUtilisateur)
        {
            if (idGroupe == null)
                return Redirect("~/Home/Index.cshtml");

            UtilisateurModel utilisateur = db.ListeUtilisateurs.Find(idUtilisateur);
            GroupeModel groupe = db.ListeGroupes.Find(idGroupe);

            groupe.MembresGroupe.Remove(utilisateur);
            db.SaveChanges();

            if (HttpContext.Request.UrlReferrer.ToString().Contains("Editer"))
                return RedirectToAction("Editer", new { id = idGroupe });
            else
                return RedirectToAction("Details", new { id = idGroupe });
        }

        [HttpPost]
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

        private List<GroupeModel> ListerGroupes(UtilisateurModel utilisateur)
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