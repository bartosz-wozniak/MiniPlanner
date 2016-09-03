using System.Collections.Generic;
using BusinessLogic.DtoObjects;

namespace Web.Models
{
    public class ViewOwnSchedulesViewModel
    {
        public IList<int> Schedules { get; set; }
        public IEnumerable<DtoSchedule> ScheduleDetails { get; set; }
        public string ErrorText { get; set; }
    }
}