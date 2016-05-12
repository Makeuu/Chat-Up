using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatUp.Dal
{
    public class DalUtilisateur : IDalUtilisateur
    {
        private BddContext bdd;

        public DalUtilisateur()
        {
            bdd = new BddContext();
        }

        public void CreerUtilisateur(string email, string motdepasse)
        {
            bdd.ListeUtilisateurs.Add(new UtilisateurModels { Email = email, MotDePasse = motdepasse, DateInscription = DateTime.Today });
            bdd.SaveChanges(); 
        }

        public void Dispose()
        {
            bdd.Dispose();
        }
  
        public List<UtilisateurModels> ObtientTousLesUtilisateurs()
        {
            return bdd.ListeUtilisateurs.ToList();
        }
    }
}