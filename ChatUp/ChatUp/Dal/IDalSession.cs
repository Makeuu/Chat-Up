using ChatUp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatUp.Dal
{
    public interface IDalSession
    {
        List<SessionModel> ObtientToutesLesSessions();
        SessionModel AuthentifierUtilisateur(String email, String motDePasse);
        SessionModel CreerSession(String email, String motDePasse, UtilisateurModel utilisateur);
        void SupprimerSession(SessionModel session);
        SessionModel ObtenirSession(String email);
    }
}
