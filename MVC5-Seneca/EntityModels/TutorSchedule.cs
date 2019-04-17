﻿using System;
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
        [NotMapped] public String DayName { get; set;}
        [DisplayName("Time")]
        [NotMapped] public String TimeOfDay { get; set; }
    }
}