using System;
using System.ComponentModel.DataAnnotations;

public class UtilisateurModels
{
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
    public ProfilModels Profil { get; set; }
}
