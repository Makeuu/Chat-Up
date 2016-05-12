using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatUp.Models
{
    public interface IDalUtilisateur : IDisposable
    {
        List<UtilisateurModels> ObtientTousLesUtilisateurs();
        void CreerUtilisateur(string email, string motdepasse);

    }
}