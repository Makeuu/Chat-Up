using System;
using System.ComponentModel.DataAnnotations;
public class ProfilModels
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

}
