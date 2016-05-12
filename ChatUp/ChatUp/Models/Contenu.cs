using System;

namespace ChatUp.Models
{
    public abstract class  Contenu
    {
        public DateTime DateEnvoi { get; set; }

        public Contenu(Contenu Contenu) { }
    }
}