using BusinessLogic.DtoObjects;
using DataAccess;

namespace BusinessLogic.Converters
{
    public class ScheduleConverter
    {
        public static DtoSchedule DataAccsessToDto(Preferences d)
        {
            return new DtoSchedule
            {
                Id = d.id,
                Course = CourseConverter.DataAccsessToDto(d.Courses),
                ScheduleId = d.scheduleId,
                User = UserConverter.DataAccsessToDto(d.Users)
            };
        }

        public static Preferences DtoToDataAccess(DtoSchedule d)
        {
            return new Preferences
            {
                id = d.Id,
                courseId = d.Course.Id,
                scheduleId = d.ScheduleId,
                userId = d.User.Id
            };
        }
    }
}
