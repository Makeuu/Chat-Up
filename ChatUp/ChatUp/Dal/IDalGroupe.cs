using ChatUp.Models;
using System.Collections.Generic;
using System.Drawing;

namespace ChatUp.Dal
{
    public interface IDalGroupe
    {
        GroupeModel creerGroupe(string nom, UtilisateurModel admin, List<UtilisateurModel> membres);
        bool changeAdmin(UtilisateurModel admin, int idGroupe);
        bool ajouterMembre(UtilisateurModel membre, int idGroupe);
        void ajouterMembres(List<UtilisateurModel> membres, int idGroupee);
        bool supprimerMembre(UtilisateurModel membre, int idGroupe);
        List<UtilisateurModel> voirMembres(int idGroupe);
        bool ajouterMessage(UtilisateurModel utilisateur, string message, int idGroupe);
        //bool ajouterMessage(UtilisateurModel utilisateur, Image image, int idGroupe);
        //bool ajouterMessage(UtilisateurModel utilisateur, Video video, int idGroupe);
        bool supprimerMessage(ContenuModel message, int idGroupe);
        List<PieceJointeModel> voirPiecesJointes(int idGroupe);
        List<ContenuModel> voirHistorique(int idGroupe);
        void autoriserInvitation(bool autorisation, int idGroupe);
    }
}
