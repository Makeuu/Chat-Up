using ChatUp.Dal;
using ChatUp.Models;
using Microsoft.Owin;
using Owin;
using System.Collections.Generic;
using System.Web.Security;

[assembly: OwinStartup(typeof(ChatUp.Startup))]
namespace ChatUp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            DalUtilisateur dal = new DalUtilisateur();
            List<UtilisateurModel> liste = dal.ObtientTousLesUtilisateurs();
            //ConfigureAuth(app);
        }
    }
}
