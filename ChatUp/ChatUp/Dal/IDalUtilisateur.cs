using System;
using System.Collections.Generic;
using ChatUp.Models;

namespace ChatUp.Dal
{
    public interface IDalUtilisateur : IDisposable
    {
        List<UtilisateurModel> ObtientTousLesUtilisateurs();
        void CreerUtilisateur(string email, string motdepasse);

    }
}