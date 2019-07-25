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
        //public HfedScheduleViewModel(int pointPersonId)
        //{
        //    PointPerson_Id = pointPersonId;
        //}

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
        public HfedStaff PointPerson { get; set; }

        [DisplayName("Note")]
        [DataType(DataType.MultilineText)]

        public string ScheduleNote { get; set; }

        public Boolean Request { get; set; }

        public Boolean Complete { get; set; }

        [DisplayName("Drivers")]
        public string HfedDriverIds { get; set; }  // IDs for multi-select Drivers & DropDownList 

        [DisplayName("Clients")]
        public string HfedClientIds { get; set; }  // IDs fFor multi-select Clients & DropDownList 

        [DisplayName("Volunteer Hours")]
        public int? VolunteerHours { get; set; }

        public int Location_Id { get; set; }
        public int PointPerson_Id { get; set; }
        public int Provider_Id { get; set; }
        public string[] HfedDriversArray { get; set; }
        public IEnumerable<SelectListItem> SelectedHfedDrivers { get; set; }
        public string[] HfedClientsArray { get; set; }
        public IEnumerable<SelectListItem> SelectedHfedClients { get; set; }
        public List<HfedLocation> HfedLocations { get; set; }
        public List<HfedProvider> HfedProviders { get; set; }
        public List<HfedDriver> HfedDrivers { get; set; }
        public List<HfedStaff> HfedStaffs { get; set; }
        public List<HfedClient> HfedClients { get; set; }
        public List<HfedSchedule> HfedSchedules { get; set; }
        public string ErrorMessage { get; set; }
    }
}