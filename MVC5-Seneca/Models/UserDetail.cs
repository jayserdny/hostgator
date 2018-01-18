using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MVC5_Seneca.Models;

namespace MVC5_Seneca.EntityModels
{
    public class UserDetail
    {
        public int Id { get; set; }     
      
        [Required(ErrorMessage ="This field is required.")]
        public virtual ApplicationUser ApplicationUser { get; set; }            
        
        public Boolean Active { get; set; }
        public string LoginErrorMessage { get; set; }
      
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Location")]
        public string Location { get; set; }     
    }
}