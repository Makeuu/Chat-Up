using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ChatUp.Models
{
    #region Modèle du groupe
    public class GroupeModel
    {        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdGroupe { get; set; }

        [Required]
        [StringLength(256, ErrorMessage = "Le nom du groupe doit compter entre 3 et 256 caractères.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Nom du groupe")]
        public string NomGroupe { get; set; }
        
        [DataType(DataType.Text)]
        [Display(Name = "Image du groupe")]
        public byte[] ImageGroupe { get; set; }
        
        [Display(Name = "Tout le monde peut envoyer des invitations ?")]
        public bool InvitationAutorisee { get; set; }
        
        [DataType(DataType.DateTime)]
        [Display(Name = "Date de création du groupe")]
        public DateTime DateCreationGroupe { get; set; }
        
        [ForeignKey("AdministrateurGroupe")]
        public string AdministrateurGroupeId { get; set; }
        
        [Display(Name = "Administrateur du groupe")]
        public virtual UtilisateurModel AdministrateurGroupe { get; set; }
        
        [Display(Name = "Membres du groupe")]
        public virtual List<UtilisateurModel> MembresGroupe { get; set; }
        
        [Display(Name = "Historique des messages")]
        public virtual List<MessageModel> ListeMessages { get; set; }
    }
    #endregion

    #region Modèle d'une classe contenu 
    public abstract class ContenuModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdContenu { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateEnvoi { get; set; }

        [ForeignKey("AuteurContenu")]
        public string AuteurContenuId { get; set; }

        [ForeignKey("Groupe")]
        public int GroupeId { get; set; }
        
        public virtual UtilisateurModel AuteurContenu { get; set; }
        public virtual GroupeModel Groupe { get; set; }
    }
    #endregion

    #region Modèle d'une classe message
    public class MessageModel : ContenuModel
    {
        [DataType(DataType.Text)]
        public string Message { get; set; }
    }
    #endregion

    #region Modèle d'une pièce jointe
    public class PieceJointeModel : ContenuModel
    {
        [DataType(DataType.Text)]
        public string Nom { get; set; }
    }
    #endregion
}