namespace SIM.Models
{
    using AspNet.Identity.Oracle;
     using System.ComponentModel.DataAnnotations;


    public class User : IdentityUser
    {
        [Display(Name = "Nombres")]
        [MaxLength(50, ErrorMessage = "FirstNameMaxLength")]
        public string Nombres { get; set; }

        [Display(Name = "LastName")]
        [MaxLength(50, ErrorMessage = "LastNameMaxLength")]
        public string Apellidos { get; set; }

        public string Habilitado { get; set; }
    }
}
