using System;
using System.Data.Entity;
using ChatUp.Models;

namespace ChatUp.Dal
{
    public class BddInitializer : DropCreateDatabaseAlways<BddContext>
    {
        protected override void Seed(BddContext context)
        {
            ProfilModel profil = new ProfilModel { Nom = "Mercier", Prenom = "Matthieu", Anniversaire = new DateTime(1993, 5, 31) };
            UtilisateurModel utilisateur = new UtilisateurModel { Email = "matthieu@gmail.com", DateInscription = DateTime.Now, MotDePasse = "matt", Profil = profil };

            context.ListeProfils.Add(profil);
            context.ListeUtilisateurs.Add(utilisateur);
            context.SaveChanges();

            base.Seed(context);
        }
    }
}