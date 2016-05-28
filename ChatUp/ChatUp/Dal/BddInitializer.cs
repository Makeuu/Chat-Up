using System;
using System.Data.Entity;
using ChatUp.Models;
using System.Collections.Generic;
using System.Linq;

namespace ChatUp.Dal
{
    public class BddInitializer : DropCreateDatabaseAlways<BddContext>
    {
        protected override void Seed(BddContext context)
        {
            ProfilModel profil = new ProfilModel { Nom = "Mercier", Prenom = "Matthieu", Anniversaire = new DateTime(1993, 5, 31) };
            ProfilModel profil2 = new ProfilModel { Nom = "Fernane", Prenom = "Yassine", Anniversaire = new DateTime(1991, 8, 9) };

            context.ListeProfils.Add(profil);
            context.ListeProfils.Add(profil2);
            context.SaveChanges();

            profil.IdProfil = context.ListeProfils.FirstOrDefault(pr => pr.Nom == profil.Nom && pr.Prenom == profil.Prenom && pr.Anniversaire == profil.Anniversaire).IdProfil;
            
            UtilisateurModel utilisateur = new UtilisateurModel { Email = "matthieu.mercier3105@gmail.com", DateInscription = DateTime.Now, MotDePasse = "fdp", Groupes = new List<GroupeModel>(), Profil = profil };
            UtilisateurModel utilisateur2 = new UtilisateurModel { Email = "yassine.fernane@gmail.com", DateInscription = DateTime.Now, MotDePasse = "blabla", Groupes = new List<GroupeModel>(), Profil = profil2 };

            context.ListeUtilisateurs.Add(utilisateur);
            context.ListeUtilisateurs.Add(utilisateur2);
            context.SaveChanges();

            //Test YF
            DalGroupe groupe = new DalGroupe(context);
            GroupeModel gm = groupe.creerGroupe("Groupe Test", utilisateur, new List<UtilisateurModel>());

            groupe.ajouterMembres(context.ListeUtilisateurs.ToList(), gm.IdGroupe);
            utilisateur.Groupes.Add(gm);
            utilisateur2.Groupes.Add(gm);

            groupe.changeAdmin(utilisateur2, gm.IdGroupe);
            context.SaveChanges();

            base.Seed(context);
        }
    }
}