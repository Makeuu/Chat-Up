using System;
using System.Drawing;

namespace ChatUp.Models
{
    public class Profil
    {
        public string Nom { get; set; }

        public string Prenom { get; set; }

        public DateTime Anniversaire { get; set; }
           
        public Image ImageDeprofil { get; set; }


    }
}