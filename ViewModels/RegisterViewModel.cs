using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspcoreIdentity.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display( Name = "ConfirmPassword") ]
        [Compare("Password", ErrorMessage ="Password And ConfirmPassword do not match")]
        public string ConfirmPassword { get; set; }
    }
}
