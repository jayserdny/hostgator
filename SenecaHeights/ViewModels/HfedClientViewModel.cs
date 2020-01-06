using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using MVC5_Seneca.EntityModels;

namespace MVC5_Seneca.ViewModels
{
    public class HfedClientViewModel
    {         
        public int SelectedId { get; set; }
        public List<HfedClient> HfedClients { get; set; }
        public IEnumerable <HfedLocation> HfedLocations { get; set; }
    }
}