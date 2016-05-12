using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatUp.Models
{
    public abstract class  Contenu
    {
        public DateTime DateEnvoi { get; set; }

        public Contenu(Contenu Contenu) { }
    }
}