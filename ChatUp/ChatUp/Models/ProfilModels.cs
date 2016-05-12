using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
public class ProfilModels
{
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

    public Image ImageProfil { get; set; }

}
