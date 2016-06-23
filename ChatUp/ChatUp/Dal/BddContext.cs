using System.Data.Entity;
using ChatUp.Models;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace ChatUp.Dal
{
    public class BddContext : DbContext
    {
        public BddContext() : base(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ChatUp.Models.BddContext;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")
        {
            Database.SetInitializer(new BddInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Entity<GroupeModel>()
                        .HasMany(grp => grp.MembresGroupe)
                        .WithMany(usr => usr.Groupes)
                        .Map(cs =>
                        {
                            cs.MapLeftKey("IdGroupe"); 
                            cs.MapRightKey("Email");
                            cs.ToTable("GroupeUtilisateur");
                        });
        }

        public DbSet<UtilisateurModel> ListeUtilisateurs { get; set; }
        public DbSet<ProfilModel> ListeProfils { get; set; }
        public DbSet<SessionModel>ListeSessions { get; set; }
        public DbSet<GroupeModel> ListeGroupes { get; set; }
        public DbSet<MessageModel> ListeMessages { get; set; }
        public DbSet<PieceJointeModel> ListePiecesJointes { get; set; }
    }
}