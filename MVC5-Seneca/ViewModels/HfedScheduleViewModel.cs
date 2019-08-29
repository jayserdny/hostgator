using MVC5_Seneca.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace MVC5_Seneca.ViewModels
{
    public class HfedScheduleViewModel
    {   
        public int Id { get; set; }

        [DisplayName("Date")]
        [Column(TypeName = "Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Date { get; set; }

        [DisplayName("Time")]
        public string PickUpTime { get; set; }

        [Required]
        [DisplayName("Food Provider")]
        public HfedProvider Provider { get; set; }

        [Required]
        [DisplayName("Drop Off Location")]
        public HfedLocation Location { get; set; }

        [DisplayName("Point Person")]
        public ApplicationUser PointPerson { get; set; }
        public ApplicationUser Driver { get; set; }

        [DisplayName("Note")]
        [DataType(DataType.MultilineText)]

        public string ScheduleNote { get; set; }

        public Boolean Request { get; set; }

        public Boolean Complete { get; set; }

        public Boolean  Approved { get; set; }
        public int?  Households { get; set; }

        [DisplayName("Drivers")]
        public string HfedDriverIds { get; set; }  // IDs for multi-select Drivers & DropDownList 

        [DisplayName("Clients")]
        public string HfedClientIds { get; set; }  // IDs fFor multi-select Clients & DropDownList 

        [DisplayName("Volunteer Hours")]
        public float? VolunteerHours { get; set; }

        public int Location_Id { get; set; }
        public string PointPerson_Id { get; set; }
        public string Driver_Id { get; set; }
        public int Provider_Id { get; set; }
        public string[] HfedDriversArray { get; set; }
        public IEnumerable<SelectListItem> SelectedHfedDrivers { get; set; }
        public string[] HfedClientsArray { get; set; }
        public IEnumerable<SelectListItem> SelectedHfedClients { get; set; }
        public List<HfedLocation> HfedLocations { get; set; }
        public List<HfedProvider> HfedProviders { get; set; }
        public List<ApplicationUser> HfedDrivers { get; set; }
        public List<ApplicationUser> HfedStaffs { get; set; }
        public List<HfedClient> HfedClients { get; set; }
        public List<HfedSchedule> HfedScheds { get; set; }      
        public Boolean UserIsOnSchedule { get; set; } // User is the driver on at least one delivery
        public string DriverFullName { get; set; }   // For Driver SignUp screen
        public string FormattedDay { get; set; }   // For Driver SignUp screen
        public string FormattedDate { get; set; }   // For Driver SignUp screen
    }
}                                                       