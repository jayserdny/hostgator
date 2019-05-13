using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MVC5_Seneca.EntityModels
{
    public class HfedStaff
    {
        public int Id { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }
 
        [Required]
        [DisplayName("Location")]
        public HfedLocation Location { get; set; }

        [DisplayName("Phone")]
        public string Phone { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Note")]
        [DataType(DataType.MultilineText)]
        public string StaffNote { get; set; }

        [NotMapped] public List<HfedLocation> HfedLocations { get; set; }
    }
}