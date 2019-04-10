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

        [DisplayName("Day")]
        public string DayOfWeek { get; set; }

        [DisplayName("Time")]
        public string TimeOfDay { get; set; }
    }
}