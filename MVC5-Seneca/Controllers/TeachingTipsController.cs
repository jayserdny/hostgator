using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Blob; // Namespace for Blob storage types;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.ViewModels;

namespace MVC5_Seneca.Controllers
{  
    public class TeachingTipsController : Controller
    {
        private readonly SenecaContext _db = new SenecaContext();
        // GET: TeachingTips
        public ActionResult Index()
        {
          TeachingTipsViewModel model = new TeachingTipsViewModel();
            var sortedTips = _db.TipDocuments.OrderBy(t => t.Category.Id);
            var tipCategories = _db.TipsCategories; 
            return View();
        }
        public ActionResult ReturnToDashboard()
        {
            return RedirectToAction("Index", "Home");
        }
    } 
}