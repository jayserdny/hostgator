using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC5_Seneca.EntityModels
{
    public class HfedClient
    {
        public int Id { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Date of Birth")]
        [Column(TypeName = "Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DateOfBirth { get; set; }
        
        [Required]
        [DisplayName("Location")]
        public HfedLocation Location { get; set; }

        [DisplayName("Note")]
        [DataType(DataType.MultilineText)]
        public string ClientNote { get; set; }

        [NotMapped] public List<HfedLocation> HfedLocations { get; set; }
        [NotMapped] public string FullName => $"{FirstName} {LastName}";
    }    
}   