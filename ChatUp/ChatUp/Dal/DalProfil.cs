using System;
using System.Collections.Generic;
using System.Linq;
using ChatUp.Models;

namespace ChatUp.Dal
{
    class DalProfil
    {
        private BddContext bdd;

        public DalProfil()
        {
            bdd = new BddContext();
        }

        public void CreerProfil(string nom, string prenom, DateTime anniversaire)
        {
            bdd.ListeProfils.Add(new ProfilModel { Nom = nom, Prenom = prenom, Anniversaire = anniversaire });
            bdd.SaveChanges();
        }

        public void Dispose()
        {
            bdd.Dispose();
        }

        public List<ProfilModel> ObtientTousLesUtilisateurs()
        {
            return bdd.ListeProfils.ToList();
        }
    }
}
