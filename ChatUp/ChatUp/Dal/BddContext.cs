using System.Data.Entity;

namespace ChatUp.Dal
{
    public class BddContext : DbContext
    {
        public BddContext() : base(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ChatUp.Models.BddContext;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")
        {
            System.Data.Entity.Database.SetInitializer<BddContext>(new BddInitializer());
        }
        public DbSet<UtilisateurModels> ListeUtilisateurs { get; set; }
        public DbSet<ProfilModels> ListeProfils { get; set; }

    }
}