using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BookStore.Models
{
    public class ApplicationUsers : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        [RegularExpression("^([A-PR-UWYZa-pr-uwyz](([0-9](([0-9]|[A-HJKSTUWa-hjkstuw])?)?)|([A-HK-Ya-hk-y][0-9]([0-9]|[ABEHMNPRVWXYabehmnprvwxy])?)) [0-9][ABD-HJLNP-UW-Zabd-hjlnp-uw-z]{2})", ErrorMessage = "This is not right")] 
        public string PostCode { get; set; }
        [NotMapped]
        public string Role { get; set; }



    }
}
