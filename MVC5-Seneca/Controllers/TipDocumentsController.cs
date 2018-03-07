using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.EntityModels;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.IO;
using MVC5_Seneca.ViewModels;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace MVC5_Seneca.Controllers
{
    [System.Runtime.InteropServices.Guid("A8B629A1-8217-4C96-96E3-44642DF01D2A")]
    public class TipDocumentsController : Controller
    {
        private readonly SenecaContext _db = new SenecaContext();

        // GET: TipDocuments
        public ActionResult Index()
        {
            var model = _db.TipDocuments.ToList();
            return View(model);
        }

        // GET: TipDocuments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipDocument tipDocument = _db.TipDocuments.Find(id);
            if (tipDocument == null)
            {
                return HttpNotFound();
            }
            return View(tipDocument);
        }

        // GET: TipDocuments/Create
        public ActionResult Create()
        {
            string currentUserId = User.Identity.GetUserId();
            List<SelectListItem> categoryList = new List<SelectListItem>();
            var sortedCategories = _db.TipsCategories.OrderByDescending(c => c.Name).ToList();
            foreach (TipsCategory category in sortedCategories)
            {
                categoryList.Add(new SelectListItem { Text = category.Name, Value = category.Id.ToString() });
            }

            var model = new AddEditTipDocumentViewModel();
            string errMsg = NewMethod();
            if (errMsg != null)
            {
                model.ErrorMessage = errMsg;
            }

            model.Categories = categoryList;
            model.User = _db.Users.Find(currentUserId);

            return View(model);
        }    
        private string NewMethod()
        {
            return TempData["ErrorMessage"] as string;
        }

        // POST: TipDocuments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]       
        public ActionResult Create(HttpPostedFileBase file, int? categoryId, string name)
        { 
            if (categoryId == null)
            {
                TempData["ErrorMessage"] = "Category required. Re-enter all.";
                return RedirectToAction("Create");
            }

            try
            {
                if (file != null)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    string path = Server.MapPath(" ") + "\\" + fileName;
                    path = path.Replace("\\TipDocuments", "\\UploadFiles");
                    path = path.Replace("\\", "/");
                    file.SaveAs(path);

                    CloudStorageAccount storageAccount =
                        CloudStorageAccount.Parse(Properties.Settings.Default.StorageConnectionString);
                    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                    CloudBlobContainer container = blobClient.GetContainerReference("teachingtips");
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

                    string currentUserId = User.Identity.GetUserId();
                    var tipDocument = new TipDocument
                    {
                        DocumentLink = path.Replace(@"\", "/"),
                        User = _db.Users.Find(currentUserId),
                        Category = _db.TipsCategories.Find(categoryId),
                        Name = name
                    };
                    _db.TipDocuments.Add(tipDocument);
                    _db.SaveChanges();

                    //var model = new AddEditTipDocumentViewModel();
                    //return View(model);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                var t = ex;
                return null;
            }
            TempData["ErrorMessage"] = "File not found error.";
            return RedirectToAction("Create");
        }

        // GET: TipDocuments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipDocument tipDocument = _db.TipDocuments.Find(id);
            if (tipDocument == null)
            {
                return HttpNotFound();
            }

            List<SelectListItem> categoryList = new List<SelectListItem>();
            foreach (TipsCategory tipCategory in _db.TipsCategories)
            {
                if (tipDocument.Category.Id == tipCategory.Id)
                    categoryList.Add(new SelectListItem { Text = tipCategory.Name, Value = tipCategory.Id.ToString(), Selected = true });
                else
                    categoryList.Add(new SelectListItem { Text = tipCategory.Name, Value = tipCategory.Id.ToString(), Selected = false });
            }

            tipDocument.Categories = categoryList;

            return View(tipDocument);
        }

        // POST: TipDocuments/Edit/5     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Category,DocumentLink,User")] TipDocument viewModel)
        {
            if (ModelState.IsValid)
            { 
                var tipDocument = _db.TipDocuments.Find(viewModel.Id);
                if (tipDocument != null)
                {
                    tipDocument.Name = viewModel.Name;
                    tipDocument.DocumentLink = viewModel.DocumentLink;
                    tipDocument.User = (from u in _db.Users where u.Id == viewModel.User.Id select u).Single();
                    tipDocument.Category = (from c in _db.TipsCategories where c.Id == viewModel.Category.Id select c).Single();
                }

                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // GET: TipDocuments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipDocument tipDocument = _db.TipDocuments.Find(id);
            if (tipDocument == null)
            {
                return HttpNotFound();
            }
            return View(tipDocument);
        }

        // POST: TipDocuments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipDocument tipDocument = _db.TipDocuments.Find(id);
            if (tipDocument != null) _db.TipDocuments.Remove(tipDocument);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ReturnToDashboard()
        {
            return RedirectToAction("Index", "Home");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
