using System;
using System.Data.Entity;

namespace ChatUp.Dal
{
    public class BddInitializer : DropCreateDatabaseAlways<BddContext>
    {
        protected override void Seed(BddContext context)
        {
            ProfilModels profil = new ProfilModels { Nom = "Mercier", Prenom = "Matthieu", Anniversaire = new DateTime(1993, 5, 31) };
            UtilisateurModels utilisateur = new UtilisateurModels { Email = "matthieu.mercier3105@gmail.com", DateInscription = DateTime.Now, MotDePasse = "fdp", Profil = profil };

            context.ListeProfils.Add(profil);
            context.ListeUtilisateurs.Add(utilisateur);
            context.SaveChanges();

            base.Seed(context);
        }
    }
}