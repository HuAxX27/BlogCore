using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "Nombre obligatorio")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Domicilio obligatorio")]
        [Display(Name = "Domicilio")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Ciudad obligatorio")]
        [Display(Name = "Ciudad")]
        public string City { get; set; }

        [Required(ErrorMessage = "Pais obligatorio")]
        [Display(Name = "Pais")]
        public string Country { get; set; }



    }
}
