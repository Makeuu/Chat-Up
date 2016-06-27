using System;
using System.Collections.Generic;
using System.Linq;
using ChatUp.Models;

namespace ChatUp.Dal
{
    public class DalUtilisateur : IDalUtilisateur
    {
        private BddContext bdd;

        public DalUtilisateur()
        {
            bdd = new BddContext();
        }

        public bool CreerUtilisateur(string email, string motdepasse)
        {
            bool flag = false;
            if (ObtenirUtilisateur(email) == null)
            {
                bdd.ListeUtilisateurs.Add(new UtilisateursModels { Email = email, MotDePasse = motdepasse, DateInscription = DateTime.Now });
                bdd.SaveChanges();
                flag = true;
            }
            return flag;
        }
        public UtilisateursModels ObtenirUtilisateur(string email)
        {
            return bdd.ListeUtilisateurs.FirstOrDefault(u => u.Email == email);
        }
        public void Dispose()
        {
            bdd.Dispose();
        }

        public List<UtilisateursModels> ObtientTousLesUtilisateurs()
        {
            return bdd.ListeUtilisateurs.ToList();
        }
    }
}