using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC5_Seneca.EntityModels

{
    public class DocumentType
    {
        public int Id { get; set; }

        [DisplayName("Document Type")] 
        public string Name { get; set; } 
       
    }
}