using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC5_Seneca.EntityModels;
using MVC5_Seneca.ViewModels;
using MVC5_Seneca.DataAccessLayer;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace MVC5_Seneca.Controllers
{
   public class UploadController : Controller
    {
        private SenecaContext db = new SenecaContext();
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            var model = new UploadFileViewModel();

            string errMsg = NewMethod();
            if (errMsg != null)
            {
                model.ErrorMessage = errMsg;
            }

            var sortedStudents = db.Students.OrderBy(s => s.FirstName);
            model.Students = sortedStudents.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.FirstName
            })
            .ToList();

            var documentTypes = db.DocumentTypes.ToList();
            model.DocumentTypes = db.DocumentTypes.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Name
            })
            .ToList();

            return View(model);
        }

        private string NewMethod()
        {
            return TempData["ErrorMessage"] as string;
        }

        // POST: Upload
        [HttpPost, Authorize(Roles = "Administrator")]
        public ActionResult Upload(/*UploadFileViewModel model*/HttpPostedFileBase file, int? student_Id, int? documentType_Id)
        {
            if (student_Id == null ) 
            {
                TempData["ErrorMessage"] = "Student ID AND Document Type required. Re-enter all.";
                return RedirectToAction("Index");
            }
            if (documentType_Id == null)
            {
                TempData["ErrorMessage"] = "Student ID AND Document Type required. Re-enter all.";
                return RedirectToAction("Index");
            }          

            try
            {      
                if (file != null)              
                {
                    var fileName = Path.GetFileName(file.FileName);            
                    string path =Server.MapPath(" ") + "\\" + fileName;                                    
                    path = path.Replace("\\Upload", "\\UploadFiles");
                    path = path.Replace("\\","/");
                    file.SaveAs(path);

                    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Properties.Settings.Default.StorageConnectionString);
                    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                    CloudBlobContainer container = blobClient.GetContainerReference("studentreports");
                    CloudBlockBlob blob = container.GetBlockBlobReference(fileName);

                    if (blob.Exists())
                    {
                        TempData["ErrorMessage"] = "There is already a file with this name. Re-enter all.";
                        return RedirectToAction("Index");
                    }

                    blob.Properties.ContentType = "application/pdf";     
                    using (var fileStream = System.IO.File.OpenRead(path))
                    {
                        blob.UploadFromStream(fileStream);
                    }

                    System.IO.File.Delete(path);

                    int i = path.IndexOf("UploadFiles");
                    path = path.Remove(0, i + 12);
                    var studentReport = new StudentReport
                    {
                        DocumentLink = path.Replace(@"\", "/"),
                        Student = db.Students.Find(student_Id),
                        DocumentType = db.DocumentTypes.Find(documentType_Id),
                        DocumentDate = DateTime.Now
                    };
                    db.StudentReports.Add(studentReport);
                    db.SaveChanges(); 
                    
                    return RedirectToAction("Edit","StudentReports", new {id = studentReport.Id });
                }                                                                                                 
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                var tt = ex;
                return null;
            }
        }
        public ActionResult ReturnToDashboard()
        {
            return RedirectToAction("Index", "Home");
        }

    }
}