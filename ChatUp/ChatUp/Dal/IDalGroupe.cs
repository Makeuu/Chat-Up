using ChatUp.Models;
using System.Collections.Generic;

namespace ChatUp.Dal
{
    public interface IDalGroupe
    {
        GroupeModel CreerGroupe(string nom, UtilisateurModel admin, List<UtilisateurModel> membres);
        bool ChangeAdmin(UtilisateurModel admin, int idGroupe);
        bool AjouterMembre(UtilisateurModel membre, int idGroupe);
        void AjouterMembres(List<UtilisateurModel> membres, int idGroupee);
        bool SupprimerMembre(UtilisateurModel membre, int idGroupe);
        List<UtilisateurModel> VoirMembres(int idGroupe);
        bool AjouterMessage(UtilisateurModel utilisateur, string message, int idGroupe);
        //bool ajouterMessage(UtilisateurModel utilisateur, Image image, int idGroupe);
        //bool ajouterMessage(UtilisateurModel utilisateur, Video video, int idGroupe);
        bool SupprimerMessage(MessageModel message, int idGroupe);
        List<PieceJointeModel> VoirPiecesJointes(int idGroupe);
        List<MessageModel> VoirHistorique(int idGroupe);
        void AutoriserInvitation(bool autorisation, int idGroupe);
    }
}
