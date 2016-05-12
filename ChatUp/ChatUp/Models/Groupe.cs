using System;
using System.Drawing;
using System.ComponentModel.DataAnnotations;

namespace ChatUp.Models
{
    public class Groupe
    {
        [Required]
        [StringLength(256, ErrorMessage = "Le nom du groupe doit compter entre 3 et 256 caractères.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Nom du groupee")]
        public string NomGroupe { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Image du groupe")]
        public Image ImageGroupe { get; set; }

        [Required]
        [Display(Name = "Tout le monde peut envoyer des invitations")]
        public bool InvitationAutorisee { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Date de création du groupe")]
        public DateTime DateCreationGroupe { get; set; }
    }
}