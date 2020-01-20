using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Blob; // Namespace for Blob storage types;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.EntityModels;
using MVC5_Seneca.Properties;
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
            var sortedTips = _db.TipDocuments.OrderBy(t => t.Category.Id).ToList();
            var tipsCategories = _db.TipsCategories.ToList();     
            string[] categories = new string[tipsCategories.Count];
            string[] documents = new string[sortedTips.Count];
            string[] htmlStrings = new string [sortedTips.Count + tipsCategories.Count];
            int iCategory = 0;
            int iDoc = 0;

            foreach (var cat in tipsCategories)
            { 
                iCategory += 1;
                categories[iCategory - 1] = cat.Name;
            }

            foreach (var doc in sortedTips)
            {
                iDoc +=1;
                documents[iDoc - 1] = doc.Name + " " + doc.DocumentLink;
            }
                                                  
            iDoc = -1;
            var oldCategoryName = "x";
            foreach (var cat in tipsCategories)
            {   
                if (cat.Name != oldCategoryName)
                {
                    iDoc++;                                               
                    var html = "<br/><strong>" + cat.Name + "</strong>"; 
                    htmlStrings[iDoc] = html;

                    oldCategoryName = cat.Name;

                    foreach (TipDocument t in sortedTips)
                    {
                        if (t.Category.Name == oldCategoryName)
                        {  
                            iDoc++;
                            html = t.Name + " <a href=\"/TeachingTips/ViewDocument/" + t.Id + " \" target=\"_blank\"> <img src=\"/Images/PDF10.png\" style=\"border:none\"> </a>";
                            htmlStrings[iDoc] = html;     
                        }
                    }    
                }
    
            }

            model.Documents = htmlStrings;
            return View(model);
        }

        public ActionResult ViewDocument(int? id)
        {
            var document = _db.TipDocuments.Find(id);
            var blobLink = SaSutility(document);
            return Redirect(blobLink);
        }

        public MvcHtmlString OutputHtml(string html)
        {
            return MvcHtmlString.Create((html));
        }

        public ActionResult ReturnToDashboard()
        {
            return RedirectToAction("Index", "Home");
        }

        private static string SaSutility(TipDocument document)
            // SAS == Shared Access Signature ; return a url to access report for 10 minutes:
        {                                                                                                                                                       
            var sasConstraints = new SharedAccessBlobPolicy
            {
                SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-5),
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(10),
                Permissions = SharedAccessBlobPermissions.Read
            };

            // Parse the connection string and return a reference to the storage account.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Settings.Default.StorageConnectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            // Retrieve a reference to a container.
            CloudBlobContainer container = blobClient.GetContainerReference("teachingtips");
            // Retrieve reference to a blob named "myblob".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(document.DocumentLink);

            var sasBlobToken = blockBlob.GetSharedAccessSignature(sasConstraints);

            return blockBlob.Uri + sasBlobToken;
        }  
    }
}