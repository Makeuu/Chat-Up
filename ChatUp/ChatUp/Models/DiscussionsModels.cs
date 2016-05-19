using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
namespace ChatUp.Models
{
    public class GroupeModel
    {        
        [Required]
        [StringLength(256, ErrorMessage = "Le nom du groupe doit compter entre 3 et 256 caractères.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Nom du groupe")]
        public string NomGroupe { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Image du groupe")]
        public Image ImageGroupe { get; set; }

        [Required]
        [Display(Name = "Tout le monde peut envoyer des invitations ?")]
        public bool InvitationAutorisee { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Date de création du groupe")]
        public DateTime DateCreationGroupe { get; set; }
    }

    public abstract class ContenuModel
    {
        public DateTime DateEnvoi { get; set; }

        public ContenuModel(ContenuModel Contenu) { }
    }

    public class MessageModel
    {
        public string Contenu { get; set; }

        public MessageModel(string Contenu) { }
    }

    public class PieceJointeModel
    {
        public string Nom { get; set; }

        public PieceJointeModel(string nom) { }

    }
}