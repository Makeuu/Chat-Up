using ChatUp.Dal;
using ChatUp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ChatUp.Controllers
{
    public class GroupeController : Controller
    {
        private BddContext db = new BddContext();

        // GET: GroupeModels
        public ActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewData["Authentifie"] = HttpContext.User.Identity.IsAuthenticated;
                ViewData["Succes"] = true;

                return View();
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

                List<GroupeModel> listeGroupes = db.ListeGroupes.Where(g => g.AdministrateurGroupeId == utilisateur.Email).ToList();

                ViewData["Authentifie"] = HttpContext.User.Identity.IsAuthenticated;
                ViewData["Succes"] = true;

                return PartialView(listeGroupes);
            }
            else
            {
                return Redirect("~/Home/Index");
            }
        }

        // GET: GroupeModels/Details/5
        public ActionResult Details(int? id)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Redirect("~/Home/Index");
            }

            if (id == null)
            {
                ViewData["Succes"] = false;

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            GroupeModel groupeModel = db.ListeGroupes.Find(id);

            if (groupeModel == null)
            {
                ViewData["Succes"] = false;

                return HttpNotFound();
            }

            ViewData["Authentifie"] = HttpContext.User.Identity.IsAuthenticated;
            ViewData["Succes"] = true;

            return View(groupeModel);
        }

        // GET: GroupeModels/Create
        public ActionResult Creer()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                string idUtilisateur = HttpContext.User.Identity.Name;

                var listeGroupes = db.ListeGroupes.Where(g => g.AdministrateurGroupeId == HttpContext.User.Identity.Name);

                ViewData["Authentifie"] = HttpContext.User.Identity.IsAuthenticated;
                ViewData["Succes"] = true;

                return View();
            }
            else
            {
                return Redirect("~/Home/Index");
            }
        }

        // POST: GroupeModels/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
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

                    ViewData["Authentifie"] = HttpContext.User.Identity.IsAuthenticated;
                    ViewData["Succes"] = true;

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["Authentifie"] = HttpContext.User.Identity.IsAuthenticated;
                    ViewData["Succes"] = false;

                    return View(groupeModel);
                }
            }
            else
            {
                return Redirect("~/Home/Index");
            }
        }

        // GET: GroupeModels/Edit/5
        public ActionResult Editer(int? id)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Redirect("~/Home/Index");
            }

            if (id == null)
            {
                ViewData["Succes"] = false;

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            GroupeModel groupeModel = db.ListeGroupes.Find(id);

            if (groupeModel == null)
            {
                ViewData["Succes"] = false;

                return HttpNotFound();
            }

            ViewData["Authentifie"] = HttpContext.User.Identity.IsAuthenticated;
            ViewData["Succes"] = true;

            return View(groupeModel);
        }

        // POST: GroupeModels/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
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

            ViewBag.AdministrateurGroupeId = new SelectList(db.ListeUtilisateurs, "Email", "MotDePasse", groupeModel.AdministrateurGroupeId);


            ViewData["Authentifie"] = HttpContext.User.Identity.IsAuthenticated;
            ViewData["Succes"] = true;

            return View(groupeModel);
        }

        // GET: GroupeModels/Delete/5
        public ActionResult Supprimer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            GroupeModel groupeModel = db.ListeGroupes.Find(id);

            if (groupeModel == null)
            {
                return HttpNotFound();
            }

            ViewData["Authentifie"] = HttpContext.User.Identity.IsAuthenticated;
            ViewData["Succes"] = true;

            return View(groupeModel);
        }

        // POST: GroupeModels/Delete/5
        [HttpPost, ActionName("Supprimer")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmerSuppression(int id)
        {
            GroupeModel groupeModel = db.ListeGroupes.Find(id);
            db.ListeGroupes.Remove(groupeModel);

            db.SaveChanges();

            ViewData["Authentifie"] = HttpContext.User.Identity.IsAuthenticated;
            ViewData["Succes"] = true;

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}