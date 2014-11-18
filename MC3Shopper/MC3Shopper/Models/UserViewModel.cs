using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MC3Shopper.Models
{
    public class UserViewModel
    {
        [Required]
        [Display(Name = "Code Client")]
        [RegularExpression(@"\d+", ErrorMessage = "Code client non conforme")]
        public string CodeClient { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; }

        [Display(Name = "Se souvenir de moi ?")]
        public bool RememberMe { get; set; }
    }
}