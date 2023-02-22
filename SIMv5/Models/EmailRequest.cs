using System.ComponentModel.DataAnnotations;

namespace SIM.Models
{
    public class EmailRequest
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}
