using ChatUp.Dal;
using Microsoft.Owin;
using Owin;
using System.Collections.Generic;

[assembly: OwinStartupAttribute(typeof(ChatUp.Startup))]
namespace ChatUp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            DalUtilisateur dal = new DalUtilisateur();
            List<UtilisateurModels> liste = dal.ObtientTousLesUtilisateurs();
            //test
            ConfigureAuth(app);
        }
    }
}
