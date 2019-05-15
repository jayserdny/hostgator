using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace MVC5_Seneca.EntityModels
{
    public class HfedSchedule
    {
        public int Id { get; set; }  
                                                
        [DisplayName("Date")]
        [Column(TypeName = "Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Date { get; set; }

        [DisplayName("Pick Up Time")]
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

        [NotMapped] public List<HfedDriver> SelectedDrivers { get; set; }
        [NotMapped] public List<HfedClient> SelectedClients { get; set; }
        [NotMapped] public List<HfedLocation> HfedLocations { get; set; }
        //[NotMapped] public IEnumerable<SelectListItem> HfedLocations { get; set; }
        [NotMapped] public List<HfedProvider> HfedProviders { get; set; }
        //[NotMapped] public IEnumerable<SelectListItem> HfedProviders { get; set; } 
        //[NotMapped] public IEnumerable<SelectListItem> HfedStaffs { get; set; }
        [NotMapped] public List<HfedStaff> HfedStaffs { get; set; }
        [NotMapped] public List<HfedDriver> HfedDrivers { get; set; }
        [NotMapped] public List<HfedClient> HfedClients { get; set; }
    }
}