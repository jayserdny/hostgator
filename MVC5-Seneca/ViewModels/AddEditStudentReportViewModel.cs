using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MVC5_Seneca.ViewModels;
using MVC5_Seneca.EntityModels;
using System.Web.Mvc;

namespace MVC5_Seneca.ViewModels
{
    public class AddEditStudentReportViewModel
    {
        // Input fields needed by the Create.cshtml file to display the form        
        public virtual IEnumerable<SelectListItem> Students { get; set; }
        public virtual IEnumerable<SelectListItem> DocumentTypes { get; set; }

        // Output fields used by the .cshtml file to return form results
        public int Id { get; set; }

        [DisplayName("Document Date")]
        [DisplayFormat(DataFormatString = "{0:d}")]
       
        [Required(ErrorMessage = "Please enter date.")]   
        public DateTime? DocumentDate { get; set; }

        [DataType(DataType.MultilineText)]       
        public string Comments { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Uploaded File")]
        [Required(ErrorMessage = "Please choose file to upload.")]
        public string DocumentLink { get; set; }

        public HttpPostedFileBase PostedFile { get; set; }

        [DisplayName("Student")]
        [Required(ErrorMessage = "Please select student.")]
        public virtual Student Student { get; set; }

        [DisplayName("DocumentType")]
        [Required(ErrorMessage = "Please choose document type.")]
        public virtual DocumentType DocumentType { get; set; }
    }
}