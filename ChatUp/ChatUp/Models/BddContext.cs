using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ChatUp.Models
{
    public class BddContext : DbContext
    {
        public DbSet<UtilisateurModels> ListeUtilisateurs { get; set; }
    }
}