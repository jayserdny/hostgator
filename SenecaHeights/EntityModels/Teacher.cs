using System.ComponentModel;

namespace SenecaHeights.EntityModels
{
    public class Teacher
    {  
        public int Id { get; set; }

        [DisplayName ("Last Name")]
        public string LastName { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }  
        public virtual School School { get; set; }   // School NOT MAINTAINED in this record (assigned in Student Record) 

        [DisplayName("Work Phone")]
        public string WorkPhone { get; set; }

        [DisplayName("Cell Phone")]
        public string CellPhone { get; set; }
        public string Email { get; set; }  
    }
}