using System;

namespace MVC5_Seneca
{
    public class HfedGlobals
    {
        // parameterless constructor required for static class
        //StartDate = DateTime.Today.AddDays(-(int) DateTime.Today.DayOfWeek); // default value

        public DateTime StartDate { get; set; } 
        public Boolean Request { get; set; }
        public Boolean Complete { get;  set; }

    } 
}