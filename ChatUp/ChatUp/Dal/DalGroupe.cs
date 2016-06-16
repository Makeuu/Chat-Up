using System.Collections.Generic;
using ChatUp.Models;
using System;
using System.Linq;

namespace ChatUp.Dal
{
    /// <summary>
    /// Classe permettant de manipuler les objets de type groupe
    /// </summary>
    public class DalGroupe : IDalGroupe
    {
        /// <summary>
        /// Une instance de notre base de donnée
        /// </summary>
        private BddContext bdd;

        /// <summary>
        /// Constructeur de la classe DalGroupe à partir d'un contexte existant
        /// </summary>
        public DalGroupe(BddContext ctx)
        {
            bdd = ctx;
        }

        /// <summary>
        /// Constructeur de la classe DalGroupe
        /// </summary>
        public DalGroupe()
        {
            bdd = new BddContext();
        }

        /// <summary>
        /// Constructeur pour un objet Groupe
        /// </summary>
        /// <param name="nom">Le nom du groupe</param>
        /// <param name="admin">L'utilisateur administrateur du groupe</param>
        /// <param name="membres">Une liste des membres du groupe</param>
        /// <returns>Retourne le groupe créé</returns>
        public GroupeModel CreerGroupe(string nom, UtilisateurModel admin, List<UtilisateurModel> membres)
        {
            //On crée un groupe avec les paramètres passés
            GroupeModel groupe = new GroupeModel
            {
                NomGroupe = nom,
                AdministrateurGroupeId = admin.Email,
                MembresGroupe = membres,
                ListeMessages = new List<MessageModel>(),
                DateCreationGroupe = DateTime.Now,
                InvitationAutorisee = false,
                ImageGroupe = null
            };

            //Sauvegarde du groupe créé en base
            bdd.ListeGroupes.Add(groupe);
            bdd.SaveChanges();

            //On récupère l'id du groupe et on le stocke dans l'objet à renvoyer
            groupe.IdGroupe = bdd.ListeGroupes.FirstOrDefault(grp => grp.AdministrateurGroupeId == admin.Email && grp.NomGroupe == nom).IdGroupe;

            //On ajoute les membres du groupe au nouveau groupe créé
            //ajouterMembres(membres, groupe.IdGroupe);

            return groupe;
        }
        
        /// <summary>
        /// Méthode permettant de modifier un groupe
        /// </summary>
        /// <param name="idGroupe">ID du groupe à modifier</param>
        /// <param name="groupe">Groupe modifié</param>
        public void EditerGroupe(int idGroupe, GroupeModel groupe)
        {
            try
            {
                //On tente de récuperer en base le groupe dont l'id a été passé en paramètre.
                GroupeModel grp = bdd.ListeGroupes.FirstOrDefault(g => g.IdGroupe == idGroupe);

                //Si le groupe a bien été récupéré, on le modifie et on sauvegarde en base
                if (grp != null && groupe != null)
                {
                    grp = groupe;
                    bdd.SaveChanges();
                }
            }
            catch (Exception e)
            {

            }
        }        
        
        /// <summary>
        /// Méthode permettant de changer l'administrateur d'un groupe
        /// </summary>
        /// <param name="admin">Le nouvel admin du groupe</param>
        /// <param name="idGroupe">L'id du groupe à modifier</param>
        /// <returns> Renvoi 'true' si le changement a bien été effectué, false sinon. </returns>
        public bool ChangeAdmin(UtilisateurModel nouvelAdmin, int  idGroupe)
        {
            try
            {
                //On tente de récuperer en base le groupe dont l'id a été passé en paramètre.
                GroupeModel groupe = bdd.ListeGroupes.FirstOrDefault(grp => grp.IdGroupe == idGroupe);

                //Si le groupe a bien été récupéré, on change l'admin et on sauvegarde en base
                if (groupe != null && nouvelAdmin != null)
                {
                    groupe.AdministrateurGroupeId = nouvelAdmin.Email;
                    bdd.SaveChanges();
                }
                //Sinon on renvoi 'false' 
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
            
            //Tout s'est bien passé, on renvoi 'true'
            return true;
        }

        /// <summary>
        /// Méthode permettant d'ajouter un membre à un groupe passé en paramètre
        /// </summary>
        /// <param name="membre">Le membre à ajouter au groupe</param>
        /// <param name="idGroupe">L'id du groupe auquel on veut ajouter un membre</param>
        /// <returns>Retourne un booléen, 'true' si le membre a bien été ajouté, 'false' sinon </returns>
        public bool AjouterMembre(UtilisateurModel membre, int idGroupe)
        {
            try
            {
                //On tente de récuperer en base le groupe auquel on veut ajouter le membre
                GroupeModel groupe = bdd.ListeGroupes.FirstOrDefault(grp => grp.IdGroupe == idGroupe);

                //Une fois le groupe récupéré on ajoute le membre
                if(groupe != null && membre != null)
                {
                    //Si le membre existe déjà dans le groupe, on ne l'ajoute pas et on le notifie
                    if (groupe.MembresGroupe.Contains(membre))
                    {
                        return false;
                    }
                    //Sinon on l'ajoute à la liste et on renvoi true
                    else
                    {
                        groupe.MembresGroupe.Add(membre);
                        bdd.SaveChanges();
                    }
                    
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }

            //Tout s'est bien passé, on renvoi 'true'
            return true;
        }

        /// <summary>
        /// Méthode permettant d'ajouter une liste de membre à un groupe
        /// </summary>
        /// <param name="membres">La liste des membres à ajouter</param>
        /// <param name="idGroupe">L'id du groupe auquel on ajoute les membres </param>
        public void AjouterMembres(List<UtilisateurModel> membres, int idGroupe)
        {
            try
            {
                //On tente de récuperer en base le groupe auquel on veut ajouter le membre
                GroupeModel groupe = bdd.ListeGroupes.FirstOrDefault(grp => grp.IdGroupe == idGroupe);

                //Une fois le groupe récupéré on ajoute les membres, seulement si la liste passée en paramètre n'est pas vide
                if(groupe != null && membres != null && membres.Count > 0)
                {
                    //On vérifie d'abord qu'on ajoute pas des membres déjà existants
                    foreach(UtilisateurModel u in membres)
                    {
                        if (!groupe.MembresGroupe.Contains(u))
                            groupe.MembresGroupe.Add(u);
                    }

                    //On sauvegarde les modifications en base
                    bdd.SaveChanges();
                }
            } 
            catch(Exception e)
            {

            }
        }

        /// <summary>
        /// Méthode permettant de supprimer un utilisateur d'un groupe donnée
        /// </summary>
        /// <param name="membre">Le membre à supprimer</param>
        /// <param name="idGroupe">L'id du groupe duquel on veut retirer l'utilisateur</param>
        /// <returns>Renvoi un booléen, 'true' si l'utilisateur a bien été supprimé, 'false' sinon </returns>
        public bool SupprimerMembre(UtilisateurModel membre, int idGroupe)
        {
            try
            {
                //On tente de récuperer en base le groupe auquel on veut ajouter le membre
                GroupeModel groupe = bdd.ListeGroupes.FirstOrDefault(grp => grp.IdGroupe == idGroupe);

                //Une fois le groupe récupéré on ajoute le membre
                if (groupe != null && membre != null)
                {
                    //Si le membre existe dans le groupe, on le supprime
                    if (groupe.MembresGroupe.Contains(membre))
                    {
                        groupe.MembresGroupe.Remove(membre);
                        bdd.SaveChanges();
                    }
                    //S'il n'est pas dans la liste, on ne peut pas le supprimer, on renvoi donc false
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                return false;
            }

            //Tout s'est bien passé, on renvoi 'true'
            return true;
        }

        /// <summary>
        /// Méthode permettant de récupérer la liste des membres d'un groupe
        /// </summary>
        /// <param name="idGroupe">L'id du groupe dont on veut récupérer la liste de membres</param>
        /// <returns>Retourne la liste des membres du groupe</returns>
        public List<UtilisateurModel> VoirMembres(int idGroupe)
        {
            List<UtilisateurModel> liste = new List<UtilisateurModel>();

            try
            {
                //On tente de récuperer en base le groupe auquel on veut ajouter le membre
                GroupeModel groupe = bdd.ListeGroupes.FirstOrDefault(grp => grp.IdGroupe == idGroupe);

                //Si le groupe existe, on récupère sa liste
                if(groupe != null)
                    liste = groupe.MembresGroupe.ToList();
            }
            catch (Exception e)
            {
                return null;
            }

            return liste;
        }

        /// <summary>
        /// Permet d'ajouter un message à un groupe
        /// </summary>
        /// <param name="utilisateur">Auteur du message</param>
        /// <param name="message">Objet message qu'on ajoute au groupe</param>
        /// <param name="idGroupe">Id du groupe auquel on ajoute un message</param>
        /// <returns>Retourne 'true' si le message a bien été ajouté, 'false' sinon. </returns>
        public bool AjouterMessage(UtilisateurModel utilisateur, string message, int idGroupe)
        {
            try
            {
                //On tente de récuperer en base le groupe auquel on veut ajouter le message
                GroupeModel groupe = bdd.ListeGroupes.FirstOrDefault(grp => grp.IdGroupe == idGroupe);

                //Une fois le groupe récupéré on ajoute le message
                if (groupe != null)
                {
                    groupe.ListeMessages.Add(new MessageModel
                    {
                        DateEnvoi = DateTime.Now,
                        Message = message
                    });
                    bdd.SaveChanges();
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }

            //Tout s'est bien passé, on renvoi 'true'
            return true;
        }

        /// <summary>
        /// Méthode permettant d'ajouter une pièce jointe à un groupe
        /// </summary>
        /// <param name="utilisateur">Utilisateur auteur du message</param>
        /// <param name="image">La pièce jointe à ajouter</param>
        /// <param name="idGroupe">L'id du groupe</param>
        /// <returns>Retourne 'true' si la pièce jointe a été ajoutée, 'false' sinon </returns>
        //public bool ajouterMessage(UtilisateurModel utilisateur, Image image, int idGroupe);

        /// <summary>
        /// Méthode permettant d'ajouter une pièce jointe à un groupe
        /// </summary>
        /// <param name="utilisateur">Utilisateur auteur du message</param>
        /// <param name="video">La pièce jointe à ajouter</param>
        /// <param name="idGroupe">L'id du groupe</param>
        /// <returns>Retourne 'true' si la pièce jointe a été ajoutée, 'false' sinon </returns>
        //public bool ajouterMessage(UtilisateurModel utilisateur, Video video, int idGroupe);

        /// <summary>
        /// Méthode permettant de supprimer un message d'un groupe
        /// </summary>
        /// <param name="message">Le message à supprimer</param>
        /// <param name="idGroupe">L'id du groupe duquel on veut supprimer le message</param>
        /// <returns></returns>
        public bool SupprimerMessage(MessageModel message, int idGroupe)
        {
            try
            {
                //On tente de récuperer en base le groupe auquel on veut ajouter le message
                GroupeModel groupe = bdd.ListeGroupes.FirstOrDefault(grp => grp.IdGroupe == idGroupe);

                //Une fois le groupe récupéré on ajoute le message
                if (groupe != null && message != null)
                {
                    groupe.ListeMessages.Remove(message);
                    bdd.SaveChanges();
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }

            //Tout s'est bien passé, on renvoi 'true'
            return true;
        }

        /// <summary>
        /// Méthode permettant de récuperer toutes les pièces jointes d'un groupe
        /// </summary>
        /// <param name="idGroupe">Le groupe dont on veut récuperer les pièces jointes</param>
        /// <returns>Retourne la liste des pièces jointes</returns>
        public List<PieceJointeModel> VoirPiecesJointes(int idGroupe)
        {
            List<PieceJointeModel> pjs = new List<PieceJointeModel>();

            try
            {
                //On tente de récuperer en base le groupe auquel on veut récuperer les pièces jointes
                GroupeModel groupe = bdd.ListeGroupes.FirstOrDefault(grp => grp.IdGroupe == idGroupe);

                //Une fois le groupe récupéré on récupère la liste
                if (groupe != null)
                {
                   foreach(ContenuModel cm in groupe.ListeMessages)
                    {
                        if (cm.GetType() == typeof(PieceJointeModel))
                            pjs.Add((PieceJointeModel)cm);
                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }
            
            return pjs;
        }

        /// <summary>
        /// Méthode permettant de récupèrer tout l'historique d'un groupe
        /// </summary>
        /// <param name="idGroupe">L'id du groupe dont on veut récupérer l'historique</param>
        /// <returns>Retourne la liste des messages du groupe</returns>
        public List<MessageModel> VoirHistorique(int idGroupe)
        {
            List<MessageModel> histo = new List<MessageModel>();

            try
            {
                //On tente de récuperer en base le groupe auquel on veut récuperer l'historique
                GroupeModel groupe = bdd.ListeGroupes.FirstOrDefault(grp => grp.IdGroupe == idGroupe);

                //Une fois le groupe récupéré on récupère la liste
                if (groupe != null)
                {
                    histo = groupe.ListeMessages.ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }

            return histo;
        }

        /// <summary>
        /// Méthode permettant de determiner si tout le monde peut inviter de nouveaux membres ou seulement l'admin
        /// </summary>
        /// <param name="autorisation">Booléen : true si tout le monde peut inviter des membres, false sinon</param>
        /// <param name="idGroupe">L'id du groupe</param>
        public void AutoriserInvitation(bool autorisation, int idGroupe)
        {
            try
            {
                //On tente de récuperer en base le groupe auquel on veut récuperer l'historique
                GroupeModel groupe = bdd.ListeGroupes.FirstOrDefault(grp => grp.IdGroupe == idGroupe);

                //Une fois le groupe récupéré on récupère la liste
                if (groupe != null)
                {
                    groupe.InvitationAutorisee = autorisation;
                    bdd.SaveChanges();
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}