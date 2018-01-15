using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MVC5_Seneca.Models;

namespace MVC5_Seneca.EntityModels
{
    public class User
    {
        public int Id { get; set; }
        [DisplayName("User Name")]
        [Required(ErrorMessage ="This field is required.")]
        public string Name { get; set; }
                
        [Required(ErrorMessage = "This field is required.")]
        public string PasswordSalt { get; set; }

        [DisplayName("Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "This field is required.")]
        public string PasswordHash { get; set; }          
        public Boolean Active { get; set; }
        public string LoginErrorMessage { get; set; }
        //public virtual ICollection<UserRole> Roles { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }


        [DisplayName("Last Name")]
        public string LastName { get; set; }


        [DisplayName("Address")]
        public string Address { get; set; }


        [DisplayName("City-State-Zip")]
        public string CityStateZip { get; set; }


        [DisplayName("Home Phone")]
        public string HomePhone { get; set; }


        [DisplayName("Cell Phone")]
        public string CellPhone { get; set; }


        [DisplayName("Email")]
        public string Email { get; set; }
    }
}