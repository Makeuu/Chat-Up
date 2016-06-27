using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ChatUp.Dal;
using ChatUp.Models;

namespace ChatUp.Views.Utilisateur
{
    public class ProfilModelsController : Controller
    {
        private BddContext db = new BddContext();

        // GET: ProfilModels/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                string idUtilisateur = HttpContext.User.Identity.Name;
                UtilisateursModels utilisateur = db.ListeUtilisateurs.Find(idUtilisateur);

                id = utilisateur.Profil.ProfilId;
            }

            ProfilModel profilModel = await db.ListeProfils.FindAsync(id);

            if (profilModel == null)
            {
                return HttpNotFound();
            }

            return View(profilModel);
        }
        public async Task<ActionResult> DetailsAmis(int? id)
        {
            if (id == null)
            {
                string idUtilisateur = HttpContext.User.Identity.Name;
                UtilisateursModels utilisateur = db.ListeUtilisateurs.Find(idUtilisateur);

                id = utilisateur.Profil.ProfilId;
            }

            ProfilModel profilModel = await db.ListeProfils.FindAsync(id);

            if (profilModel == null)
            {
                return HttpNotFound();
            }

            return View(profilModel);
        }

        // GET: ProfilModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProfilModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ProfilId,Nom,Prenom,Anniversaire")] ProfilModel profilModel)
        {
            if (ModelState.IsValid)
            {
                db.ListeProfils.Add(profilModel);
                await db.SaveChangesAsync();
                return RedirectToAction("Details", new { id = profilModel.ProfilId});
            }

            return View(profilModel);
        }

        // GET: ProfilModels/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProfilModel profilModel = await db.ListeProfils.FindAsync(id);            
            if (profilModel == null)
            {
                return HttpNotFound();
            }
            return View(profilModel);
        }

        // POST: ProfilModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ProfilId,Nom,Prenom,Anniversaire")] ProfilModel profilModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(profilModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", new { id = profilModel.ProfilId });
            }
            return View(profilModel);
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
