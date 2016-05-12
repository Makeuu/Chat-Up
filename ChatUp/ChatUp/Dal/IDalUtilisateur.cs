using System;
using System.Collections.Generic;

namespace ChatUp.Dal
{
    public interface IDalUtilisateur : IDisposable
    {
        List<UtilisateurModels> ObtientTousLesUtilisateurs();
        void CreerUtilisateur(string email, string motdepasse);

    }
}