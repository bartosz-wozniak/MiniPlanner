using System.Collections.Generic;
using BusinessLogic.DtoObjects;

namespace Web.Models
{
    public class OfferedCoursesViewModel
    {
        public IEnumerable<DtoCourse> Courses { get; set; }
    }
}