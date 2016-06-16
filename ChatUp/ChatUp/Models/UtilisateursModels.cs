using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        
        [ForeignKey("Profil")]
        public int IdProfil { get; set; }
        public virtual ProfilModel Profil { get; set; }
        
        public virtual List<GroupeModel> Groupes { get; set; }
    }

    public class ProfilModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProfilId { get; set; }
        
        [DataType(DataType.Text)]
        [Display(Name = "Nom")]
        public string Nom { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Prénom")]
        public string Prenom { get; set; }
        
        [Display(Name = "Date d'anniversaire")]
        public DateTime Anniversaire { get; set; }

        //public Image ImageDeprofil { get; set; }
    }

    public class SessionModel
    {
        [Key]
        [Required]
        [Display(Name = "Adresse Email")]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string MotDePasse { get; set; }
        public UtilisateurModel Utilisateur { get; set; }
        public bool Authentifie { get; set; }
    }

    public class EtatModel
    {

    }
}