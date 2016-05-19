using System;
using System.Collections.Generic;
using ChatUp.Models;

namespace ChatUp.Dal
{
    public interface IDalProfil : IDisposable
    {
        List<ProfilModel> ObtientTousLesProfils();
        void CreerProfil(string nom, string prenom, DateTime anniversaire);
    }
}
