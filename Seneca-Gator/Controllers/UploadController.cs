using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC5_Seneca.EntityModels;
using MVC5_Seneca.ViewModels;
using MVC5_Seneca.DataAccessLayer;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using MVC5_Seneca.Properties;

namespace MVC5_Seneca.Controllers
{
   public class UploadController : Controller
    {
        private readonly SenecaContext _db = new SenecaContext();
        [Authorize(Roles = "Active")]
        public ActionResult Index()
        {
            var model = new UploadFileViewModel();

            string errMsg = NewMethod();
            if (errMsg != null)
            {
                model.ErrorMessage = errMsg;
            }

            var sortedStudents = _db.Students.OrderBy(s => s.FirstName);
            model.Students = sortedStudents.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.FirstName
            })
            .ToList();

            model.DocumentTypes = _db.DocumentTypes.Select(s => new SelectListItem
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
        public ActionResult Upload(HttpPostedFileBase file, int? studentId, int? documentTypeId)
        {
            if (studentId == null ) 
            {
                TempData["ErrorMessage"] = "Student ID AND Document Type required. Re-enter all.";
                return RedirectToAction("Index");
            }
            if (documentTypeId == null)
            {
                TempData["ErrorMessage"] = "Student ID AND Document Type required. Re-enter all.";
                return RedirectToAction("Index");
            }          

            try
            {
                if (file != null)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    string path = Server.MapPath(" ") + "\\" + fileName;
                    path = path.Replace("\\Upload", "\\UploadFiles");
                    path = path.Replace("\\", "/");
                    file.SaveAs(path);

                    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Settings.Default.StorageConnectionString);
                    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                    CloudBlobContainer container = blobClient.GetContainerReference("studentreports");
                    CloudBlockBlob blob = container.GetBlockBlobReference(fileName);

                    if (blob.Exists())
                    {
                        TempData["ErrorMessage"] = "There is already a file with this name. Re-enter all.";
                        return RedirectToAction("Index");
                    }

                    if (fileName != null && fileName.ToUpper().Substring(fileName.Length - 3, 3) == "MP4")
                    {
                        blob.Properties.ContentType = "video/mp4";
                    }
                    else
                    {
                        blob.Properties.ContentType = "application/pdf";
                    }

                    using (var fileStream = System.IO.File.OpenRead(path))
                    {
                        blob.UploadFromStream(fileStream);
                    }

                    System.IO.File.Delete(path);

                    int i = path.IndexOf("UploadFiles", StringComparison.Ordinal);
                    path = path.Remove(0, i + 12);

                    var studentReport = new StudentReport
                    {
                        DocumentLink = path.Replace(@"\", "/"),
                        Student = _db.Students.Find(studentId),
                        DocumentType = _db.DocumentTypes.Find(documentTypeId),
                        DocumentDate = DateTime.Now
                    };
                    _db.StudentReports.Add(studentReport);
                    _db.SaveChanges(); 
                    
                    return RedirectToAction("Edit","StudentReports", new {id = studentReport.Id });
                } // If(File != null)
                if (User.IsInRole("Administrator"))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index", "Home"); // Dashboard                 
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public ActionResult ReturnToDashboard()
        {
            return RedirectToAction("Index", "Home");
        }

    }
}