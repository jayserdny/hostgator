using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MVC5_Seneca.Models;
using MVC5_Seneca.EntityModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

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