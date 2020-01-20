using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC5_Seneca.EntityModels
{
    public class TutorSchedule
    {
        public int Id { get; set; }

        [DisplayName("Tutor")]
        public ApplicationUser Tutor { get; set; }

        [DisplayName("Student")]
        public Student  Student { get; set; }                                                                  
        public int DayOfWeekIndex { get; set; } // DayOfWeek needs to be sortable for schedule
        public int MinutesPastMidnight { get; set; }  // TimeOfDay needs to be sortable for schedule

        [DisplayName("Day")]
        [NotMapped] public string DayName { get; set;}
        [DisplayName("Time")]
        [NotMapped] public string TimeOfDay { get; set; }
        [NotMapped] public List<TutorSchedule> TutorSchedules { get; set; }
    }
}