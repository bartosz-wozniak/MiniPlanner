using BusinessLogic.DtoObjects;
using DataAccess;

namespace BusinessLogic.Converters
{
    public class CourseConverter
    {
        public static DtoCourse DataAccsessToDto(Courses d)
        {
            return new DtoCourse
            {
                Id = d.id,
                Day = d.day,
                EndHour = d.endHour,
                Limit = d.limit,
                Name = d.name,
                StartHour = d.startHour
            };
        }

        public static Courses DtoToDataAccess(DtoCourse d)
        {
            return new Courses
            {
                id = d.Id,
                startHour = d.StartHour,
                endHour = d.EndHour,
                name = d.Name,
                day = d.Day,
                limit = d.Limit
            };
        }
    }
}
