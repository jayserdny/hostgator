using System.ComponentModel;

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
    }
}