using System;
using System.Collections.Generic;
using ChatUp.Models;

namespace ChatUp.Dal
{
    public interface IDalUtilisateur : IDisposable
    {
        List<UtilisateurModel> ObtientTousLesUtilisateurs();
        bool CreerUtilisateur(string email, string motdepasse);
        UtilisateurModel ObtenirUtilisateur(string email);
    }
}