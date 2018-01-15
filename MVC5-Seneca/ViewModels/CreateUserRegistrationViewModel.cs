using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MVC5_Seneca.Models;
using MVC5_Seneca.EntityModels;
using System.Web.Mvc;

namespace MVC5_Seneca.ViewModels
{
    public class CreateUserRegistrationViewModel
    {
        // Input fields needed by the Create.cshtml file to display the form


        // Output fields used by the .cshtml file to return form results

        [DisplayName("User Name")]
        [Required(ErrorMessage = "This field is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Password")]    
        public string PasswordHash { get; set; }

        [HiddenInput(DisplayValue = false)]
        [DefaultValue("0")]
        public string PasswordSalt { get; set; }


        [DisplayName("First Name")]
        public string FirstName { get; set; }


        [DisplayName("Last Name")]
        public string LastName { get; set; }


        [DisplayName("Address")]
        public string Address { get; set; }


        [DisplayName("City-State-Zip")]
        public string CityStateZip { get; set; }


        [DisplayName("Home Phone")]
        [StringLength(13, MinimumLength = 10)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number.")]
        public string HomePhone { get; set; }


        [DisplayName("Cell Phone")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(13, MinimumLength = 10)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number.")]
        public string CellPhone { get; set; }


        [DisplayName("Email")]
        [EmailAddress]
        [Required(ErrorMessage = "This field is required.")]
        public string Email { get; set; }

        public string LoginErrorMessage { get; set; }
    }
}