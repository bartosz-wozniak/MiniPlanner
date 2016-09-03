using System.Collections.Generic;
using BusinessLogic.DtoObjects;

namespace Web.Models
{
    public class CreateScheduleViewModel
    {
        public IEnumerable<DtoCourse> CoursesToChoose { get; set; }

        public IEnumerable<DtoCourse> CoursesChosen { get; set; }

        public string ErrorText { get; set; }
    }
}