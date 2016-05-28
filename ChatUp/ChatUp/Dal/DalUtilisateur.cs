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

        public void CreerUtilisateur(string email, string motdepasse)
        {
            bdd.ListeUtilisateurs.Add(new UtilisateurModel { Email = email, MotDePasse = motdepasse, DateInscription = DateTime.Now, Groupes = new List<GroupeModel>() });
            bdd.SaveChanges(); 
        }

        public void Dispose()
        {
            bdd.Dispose();
        }
        
        public List<UtilisateurModel> ObtientTousLesUtilisateurs()
        {
            return bdd.ListeUtilisateurs.ToList();
        }
    }
}