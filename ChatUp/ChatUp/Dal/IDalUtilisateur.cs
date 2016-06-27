using System;
using System.Collections.Generic;
using ChatUp.Models;

namespace ChatUp.Dal
{
    public interface IDalUtilisateur : IDisposable
    {
        List<UtilisateursModels> ObtientTousLesUtilisateurs();
        bool CreerUtilisateur(string email, string motdepasse);
        UtilisateursModels ObtenirUtilisateur(string email);
    }
}