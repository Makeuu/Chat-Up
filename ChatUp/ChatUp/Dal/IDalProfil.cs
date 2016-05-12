using System;
using System.Collections.Generic;

namespace ChatUp.Dal
{
    public interface IDalProfil : IDisposable
    {
        List<ProfilModels> ObtientTousLesProfils();
        void CreerProfil(string nom, string prenom, DateTime anniversaire);
    }
}
