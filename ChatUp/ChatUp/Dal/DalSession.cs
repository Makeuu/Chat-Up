using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChatUp.Models;

namespace ChatUp.Dal
{
    public class DalSession : IDalSession
    {
        private BddContext bdd;
        public DalSession()
        {
            bdd = new BddContext();
        }

        public SessionModel CreerSession(String email, String motDePasse, UtilisateurModel utilisateur)
        {
            SessionModel session = new SessionModel { Email = email, MotDePasse = motDePasse,
                Utilisateur = utilisateur };
            session.Authentifie = true;
            bdd.ListeSessions.Add(session);
            bdd.SaveChanges();
            return session;
        }

        public SessionModel AuthentifierUtilisateur(String email, String motDePasse)
        {
            UtilisateurModel UtilisateurSession = bdd.ListeUtilisateurs.FirstOrDefault(u => u.Email == email
            && u.MotDePasse == motDePasse);

            return CreerSession(email, motDePasse, UtilisateurSession);
        }

        public List<SessionModel> ObtientToutesLesSessions()
        {
            return bdd.ListeSessions.ToList();
        }

        public SessionModel ObtenirSession(String email)
        {
            return bdd.ListeSessions.FirstOrDefault(s => s.Email == email);
        }
        public void SupprimerSession(SessionModel session)
        {
            bdd.ListeSessions.Remove(session);
            bdd.SaveChanges();
        }
        public void Dispose()
        {
            bdd.Dispose();
        }
    }
}