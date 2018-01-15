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
    public class UploadFileViewModel
    {
        public HttpPostedFileBase PostedFile { get; set; }
    }
}