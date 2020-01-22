using System.Linq;
using System.Web.Mvc;                                                                                                     
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.EntityModels;          
using MVC5_Seneca.ViewModels;                 

namespace MVC5_Seneca.Controllers
{
    public class TeachingTipsController : Controller
    {
        private readonly SenecaContext _db = new SenecaContext();
        // GET: TeachingTips
        public ActionResult Index()
        {   
            var model = new TeachingTipsViewModel();
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
            if (document != null)
            {   
                var path = "~/TeachingTipsFiles/" + document.DocumentLink;
                return File(path, "application/pdf", document.DocumentLink);
            }   
            return (RedirectToAction( "Index", "TeachingTips"));        
        }

        public MvcHtmlString OutputHtml(string html)
        {
            return MvcHtmlString.Create((html));
        }

        public ActionResult ReturnToDashboard()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}