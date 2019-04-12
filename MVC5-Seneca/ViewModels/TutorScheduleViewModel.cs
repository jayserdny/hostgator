using MVC5_Seneca.EntityModels;
using System.Collections.Generic;
using System.ComponentModel;

namespace MVC5_Seneca.ViewModels
{
    public class TutorScheduleViewModel
    {
        public int Id { get; set; }

        [DisplayName("Tutor")]
        public ApplicationUser Tutor { get; set; }

        [DisplayName("Student")]
        public Student Student { get; set; }
        public string DayName { get; set; }
        public string TimeOfDay { get; set; }
        public string ErrorMessage { get; set; }

        public virtual List<ApplicationUser> Tutors { get; set; }
        public virtual List<Student> Students { get; set; }  
        public virtual List<string> DaysList { get; set; }
        public virtual List<string> TimesList { get; set; }    
    }
}