using System;

namespace MVC5_Seneca.ViewModels
{
    public class EditStudentViewModel : IDisposable
    {
        // Fields used by Edit/Create to match the database
        public int Id { get; set; } 
        public string FirstName { get; set; }      
        public string Gender { get; set; }      
        public DateTime BirthDate { get; set; }
        public int Parent_Id { get; set; }
        public int School_Id { get; set; }

        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }
    }
}