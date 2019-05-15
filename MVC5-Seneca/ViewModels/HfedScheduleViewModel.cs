using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using MVC5_Seneca.EntityModels;

namespace MVC5_Seneca.ViewModels
{
    public class HfedScheduleViewModel
    {
        public int Id { get; set; } 
        public DateTime Date { get; set; }
        public string PickUpTime { get; set; }                                        
        public Boolean Request { get; set; }  
        public Boolean Complete { get; set; }
        public string ScheduleNote { get; set; }   
       
        public int Location_Id { get; set; }
        public int PointPerson_Id { get; set; }
        public int Provider_Id { get; set; }

    }
}