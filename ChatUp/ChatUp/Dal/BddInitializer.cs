using System;
using System.Data.Entity;
using ChatUp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;

namespace ChatUp.Dal
{
    public class BddInitializer : DropCreateDatabaseAlways<BddContext>
    {
        protected override void Seed(BddContext context)
        {
            peuplerBase(context);

            ProfilModel profil = new ProfilModel { Nom = "Mercier", Prenom = "Matthieu", Anniversaire = new DateTime(1993, 5, 31) };
            ProfilModel profil2 = new ProfilModel { Nom = "Fernane", Prenom = "Yassine", Anniversaire = new DateTime(1991, 8, 9) };

            context.ListeProfils.Add(profil);
            context.ListeProfils.Add(profil2);
            context.SaveChanges();

            profil.ProfilId = context.ListeProfils.FirstOrDefault(pr => pr.Nom == profil.Nom && pr.Prenom == profil.Prenom && pr.Anniversaire == profil.Anniversaire).ProfilId;

            UtilisateurModel utilisateur = new UtilisateurModel { Email = "matthieu.mercier3105@gmail.com", DateInscription = DateTime.Now, MotDePasse = "fdp", Groupes = new List<GroupeModel>(), Profil = profil };
            UtilisateurModel utilisateur2 = new UtilisateurModel { Email = "yassine.fernane@gmail.com", DateInscription = DateTime.Now, MotDePasse = "blabla", Groupes = new List<GroupeModel>(), Profil = profil2 };
            UtilisateurModel utilisateur3 = new UtilisateurModel { Email = "yaya@moi.fr", DateInscription = DateTime.Now, MotDePasse = "yaya", Groupes = new List<GroupeModel>(), Profil = profil };

            context.ListeUtilisateurs.Add(utilisateur);
            context.ListeUtilisateurs.Add(utilisateur2);
            context.ListeUtilisateurs.Add(utilisateur3);
            context.SaveChanges();

            //Test YF
            GroupeModel grp = new GroupeModel
            {
                NomGroupe = "Groupe Test",
                AdministrateurGroupe = utilisateur2,
                MembresGroupe = new List<UtilisateurModel>(),
                ListeMessages = new List<MessageModel>(),
                DateCreationGroupe = DateTime.Now,
                ImageGroupe = null,
                InvitationAutorisee = true
            };

            context.ListeGroupes.Add(grp);


            utilisateur2.ListeAmis = new List<UtilisateurModel>();
            utilisateur2.ListeAmis.Add(utilisateur);
            utilisateur2.ListeAmis.Add(utilisateur3);

            grp.MembresGroupe.Add(utilisateur);
            context.SaveChanges();

            //Ajout d'amis fictifs
            creerListeAmis(context);

            ajouterMembreDansGroupe(context, utilisateur.Email);
            ajouterMembreDansGroupe(context, utilisateur2.Email);
            ajouterMembreDansGroupe(context, utilisateur3.Email);

            base.Seed(context);
        }

        private void peuplerBase(BddContext db)
        {
            //Création d'utilisateurs fictifs
            creerUtilisateur(db, "Sarah", "Croche", new DateTime(1991, 02, 28));
            creerUtilisateur(db, "Elvira", "Gauche", new DateTime(1987, 09, 12));
            creerUtilisateur(db, "Yvon", "Enbavé", new DateTime(1983, 03, 1));
            creerUtilisateur(db, "Jean", "Némar", new DateTime(1978, 12, 7));
            creerUtilisateur(db, "Larry", "Bambelle", new DateTime(1995, 08, 09));
            creerUtilisateur(db, "Bart", "Tabac", new DateTime(1993, 06, 04));
            creerUtilisateur(db, "Al", "Kollyck", new DateTime(1988, 10, 12));
            creerUtilisateur(db, "Marie", "Juanna", new DateTime(1998, 05, 07));
            creerUtilisateur(db, "Paul", "Ochon", new DateTime(1991, 02, 15));
            creerUtilisateur(db, "Alain", "Terieur", new DateTime(1987, 09, 26));

            db.SaveChanges();

            //Création de groupes fictifs
            string[] nomGroupes =
            {
                "Famille", "Boulot", "Projet", "Anniversaire", "Orga Restau", "Qui pour un cine", "Nouvel an"
            };

            Random rnd = new Random();

            for (int j = 0; j < nomGroupes.Length; j++)
            {
                UtilisateurModel utilisateur = db.ListeUtilisateurs.ToList()[rnd.Next(db.ListeUtilisateurs.Count())];                
                string nom = nomGroupes[j];

                creerGroupe(db, nom, utilisateur, Convert.ToBoolean(rnd.Next() % 2));
            }

            db.SaveChanges();
        }

        private void creerUtilisateur(BddContext db, string prenom, string nom, DateTime anniversaire)
        {
            ProfilModel profil = new ProfilModel
            {
                Anniversaire = anniversaire,
                Nom = nom,
                Prenom = prenom
            };

            UtilisateurModel utilisateur = new UtilisateurModel
            {
                DateInscription = DateTime.Now,
                Email = prenom + "." + nom + "@chatup.fr",
                Groupes = new List<GroupeModel>(),
                ListeAmis = new List<UtilisateurModel>(),
                MotDePasse = "chatup",
                Profil = profil
            };

            db.ListeUtilisateurs.Add(utilisateur);
            db.SaveChanges();
        }

        private void creerGroupe(BddContext db, string nom, UtilisateurModel admin, bool invit)
        {
            Random rnd = new Random();
            List<UtilisateurModel> listeMembres = new List<UtilisateurModel>();
            int nbUtilisateur = db.ListeUtilisateurs.Count(),
                nbMembre = rnd.Next(4, nbUtilisateur);

            for (int i = 0; i < nbMembre; i++)
            {
                UtilisateurModel membre = db.ListeUtilisateurs.ToList()[rnd.Next(1, nbUtilisateur)];

                while (listeMembres.Contains(membre) || membre.Email.Equals(admin))
                    membre = db.ListeUtilisateurs.ToList()[rnd.Next(nbUtilisateur)];

                listeMembres.Add(membre);
            }

            GroupeModel groupe = new GroupeModel
            {
                AdministrateurGroupe = admin,
                DateCreationGroupe = DateTime.Now,
                ImageGroupe = null,
                InvitationAutorisee = invit,
                ListeMessages = new List<MessageModel>(),
                MembresGroupe = listeMembres,
                NomGroupe = nom
            };

            db.ListeGroupes.Add(groupe);
            db.SaveChanges();

            creerDiscussion(db, groupe.IdGroupe);
        }

        private void creerDiscussion(BddContext db, int idGroupe)
        {
            GroupeModel grp = db.ListeGroupes.Find(idGroupe);

            string urlDiscussion = @"C:\Users\Yassine\Source\Repos\Chat-Up\ChatUp\ChatUp\Content\discussions\" + grp.NomGroupe + ".txt";
            string[] discussion = File.ReadAllLines(urlDiscussion);

            for (int i = 0; i < discussion.Length; i++)
            {
                creerMesage(db, grp.IdGroupe, discussion[i]);
            }
        }

        private void creerMesage(BddContext db, int idGroupe, string contenu)
        {
            UtilisateurModel utilisateur;
            GroupeModel grp = db.ListeGroupes.Find(idGroupe);
            string[] tabMsg = contenu.Split(';');
            int num;
            int.TryParse(tabMsg[0], out num);

            if (num > grp.MembresGroupe.Count())
                num = num % grp.MembresGroupe.Count();

            if (num == 0)
                utilisateur = grp.AdministrateurGroupe;
            else
                utilisateur = db.ListeUtilisateurs.ToList()[num - 1];

            MessageModel msg = new MessageModel
            {
                AuteurContenu = utilisateur,
                DateEnvoi = DateTime.Now,
                Groupe = grp,
                Message = tabMsg[1]
            };

            grp.ListeMessages.Add(msg);
            db.SaveChanges();
        }

        private void creerListeAmis(BddContext db)
        {
            List<UtilisateurModel> liste = db.ListeUtilisateurs.ToList();

            foreach (UtilisateurModel utilisateur in liste)
            {
                if (utilisateur.ListeAmis == null)
                    utilisateur.ListeAmis = new List<UtilisateurModel>();

                foreach (UtilisateurModel ami in liste)
                {
                    if (ami.Email != utilisateur.Email && !utilisateur.ListeAmis.Contains(ami))
                        utilisateur.ListeAmis.Add(ami);
                }
            }
        }

        private void ajouterMembreDansGroupe(BddContext db, string idUtilisateur)
        {
            List<GroupeModel> listeGrp = db.ListeGroupes.ToList();
            UtilisateurModel utilisateur = db.ListeUtilisateurs.Find(idUtilisateur);

            foreach (GroupeModel grp in listeGrp)
            {
                if (!grp.MembresGroupe.Contains(utilisateur) && grp.AdministrateurGroupe.Email != utilisateur.Email)
                    grp.MembresGroupe.Add(utilisateur);
            }

            db.SaveChanges();
        }
    }
}