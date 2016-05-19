using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace ChatUp.Models
{
    public class UtilisateurModel
    {
        [Key]
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Adresse Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string MotDePasse { get; set; }
        public DateTime DateInscription { get; set; }
        public ProfilModel Profil { get; set; }
    }

    public class ProfilModel
    {
        [Key]
        public int ProfilId { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Nom")]
        public string Nom { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Prénom")]
        public string Prenom { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date d'anniversaire")]
        public DateTime Anniversaire { get; set; }

        //public Image ImageDeprofil { get; set; }
    }

    public class SessionModel
    {

    }

    public class EtatModel
    {

    }
}