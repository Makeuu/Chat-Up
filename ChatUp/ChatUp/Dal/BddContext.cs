using System.Data.Entity;
using ChatUp.Models;

namespace ChatUp.Dal
{
    public class BddContext : DbContext
    {
        public BddContext() : base(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ChatUp.Models.BddContext;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")
        {
            Database.SetInitializer<BddContext>(new BddInitializer());
        }

        public DbSet<UtilisateurModel> ListeUtilisateurs { get; set; }
        public DbSet<ProfilModel> ListeProfils { get; set; }
    }
}