using System.Collections.Generic;
using MVC5_Seneca.EntityModels;

namespace MVC5_Seneca.ViewModels
{
    public class TeachingTipsViewModel
    {
        public ICollection<TipsCategory> Categories { get; set; }
        public ICollection<string> Documents { get; set; }
    }
}