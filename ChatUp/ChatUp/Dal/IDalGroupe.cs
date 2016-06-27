using ChatUp.Models;
using System.Collections.Generic;

namespace ChatUp.Dal
{
    public interface IDalGroupe
    {
        GroupeModel CreerGroupe(string nom, UtilisateursModels admin, List<UtilisateursModels> membres);
        bool ChangeAdmin(UtilisateursModels admin, int idGroupe);
        bool AjouterMembre(UtilisateursModels membre, int idGroupe);
        void AjouterMembres(List<UtilisateursModels> membres, int idGroupee);
        bool SupprimerMembre(UtilisateursModels membre, int idGroupe);
        List<UtilisateursModels> VoirMembres(int idGroupe);
        bool AjouterMessage(UtilisateursModels utilisateur, string message, int idGroupe);
        //bool ajouterMessage(UtilisateurModel utilisateur, Image image, int idGroupe);
        //bool ajouterMessage(UtilisateurModel utilisateur, Video video, int idGroupe);
        bool SupprimerMessage(MessageModel message, int idGroupe);
        List<PieceJointeModel> VoirPiecesJointes(int idGroupe);
        List<MessageModel> VoirHistorique(int idGroupe);
        void AutoriserInvitation(bool autorisation, int idGroupe);
    }
}
