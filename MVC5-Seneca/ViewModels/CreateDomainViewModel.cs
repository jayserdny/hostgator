using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MVC5_Seneca.EntityModels;

namespace MVC5_Seneca.ViewModels

{
    public class CreateDomainViewModel // Create Domain: show Classes dropdown list
    {
        // Input fields needed by the Create.cshtml file to display the form
        public List<Class> AllClasses;

        // Output fields used by the .cshtml file to return form results
        [DisplayName("Class")]
        public int Class_Id { get; set; }    

        [DisplayName("Domain Name")]
        public string Name { get; set; }
    }
}