using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC5_Seneca.EntityModels;
using MVC5_Seneca.ViewModels;
using MVC5_Seneca.DataAccessLayer;

namespace MVC5_Seneca.Controllers
{
   public class UploadController : Controller
    {
        private SenecaContext db = new SenecaContext();
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            return View();
        }

        // POST: Upload
        [HttpPost, Authorize(Roles = "Administrator")]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            try
            {
                if (file != null)              
                {
                    //var fileName = Path.GetFileName(file.FileName);
                    //var path = Path.Combine("~/iReportFiles", fileName);
                    string trailingPath = Path.GetFileName(file.FileName);
                    //string path = Server.MapPath(" ") + "\\" + trailingPath;
                    string path = Path.Combine(Server.MapPath("iReportFiles"), trailingPath);
                    path = path.Replace("\\Upload", "");
                    file.SaveAs(path);
                   
                    int i = path.IndexOf("iReportFiles");
                    path = path.Remove(0, i - 1);
                    var studentReport = new StudentReport();
                    studentReport.DocumentLink = path.Replace(@"\", "/");
                    studentReport.Student = db.Students.Find(1);   // 1 = Id of 'Select ID'
                    studentReport.DocumentType = db.DocumentTypes.Find(1);   // 1 = I of '--Not Selected--'
                    studentReport.DocumentDate = DateTime.Now;
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

        //public ActionResult Edit(int id)
        //{
        //    StudentReport studentReport = db.StudentReports.Find(id);
        //    var viewModel = new AddEditStudentReportViewModel();    
        //    List<SelectListItem> studentList = new List<SelectListItem>();
        //    foreach (Student student in db.Students)
        //    {
        //        if (studentReport.Student.Id == student.Id)
        //            studentList.Add(new SelectListItem { Text = student.FirstName, Value = student.Id.ToString(), Selected = true });
        //        else
        //            studentList.Add(new SelectListItem { Text = student.FirstName, Value = student.Id.ToString(), Selected = false });
        //    }                                                                                              
        //    List<SelectListItem> documentTypeList = new List<SelectListItem>();
        //    foreach (DocumentType documentType in db.DocumentTypes)
        //    {
        //        if (studentReport.DocumentType.Id == documentType.Id)
        //            documentTypeList.Add(new SelectListItem { Text = documentType.Name, Value = documentType.Id.ToString(), Selected = true });
        //        else
        //            documentTypeList.Add(new SelectListItem { Text = documentType.Name, Value = documentType.Id.ToString(), Selected = false });
        //    }
        //    viewModel.Id = studentReport.Id;
        //    viewModel.Student = studentReport.Student;
        //    viewModel.Comments = studentReport.Comments;
        //    viewModel.DocumentType = studentReport.DocumentType;
        //    viewModel.DocumentDate = studentReport.DocumentDate;
        //    viewModel.DocumentLink = studentReport.DocumentLink;
        //    viewModel.DocumentTypes = documentTypeList;
        //    viewModel.Students = studentList;
        //    return View(viewModel);
        //}
    }
}