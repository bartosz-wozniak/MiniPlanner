using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Converters;
using BusinessLogic.DtoObjects;
using DataAccess;

namespace BusinessLogic.Logic
{
    public class CourseLogic
    {
        public MiniPlannerDbEntities Context { get; set; }

        public CourseLogic()
        {
            Context = new MiniPlannerDbEntities();
        }

        public async Task<ObservableCollection<DtoCourse>> GetCourses(string filter = null)
        {
            var ret = new ObservableCollection<DtoCourse>();
            using (var data = Context)
            {
                if (!string.IsNullOrWhiteSpace(filter))
                    foreach (var item in await (from item in data.Courses where item.name.Contains(filter) select item).ToListAsync())
                        ret.Add(CourseConverter.DataAccsessToDto(item));
                else
                    foreach (var item in await (from item in data.Courses select item).ToListAsync())
                        ret.Add(CourseConverter.DataAccsessToDto(item));
                return ret;
            }
        }

        public async Task<DtoCourse> GetCourse(int courseId)
        {
            try
            {
                using (var data = Context)
                    return CourseConverter.DataAccsessToDto(await (from item in data.Courses where item.id == courseId select item).FirstOrDefaultAsync());
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> SaveCourse(DtoCourse course)
        {
            try
            {
                using (var data = Context)
                {
                    var c = await (from item in data.Courses where course.Id == item.id select item).FirstOrDefaultAsync();
                    // Updating Course
                    if (c != null)
                    {
                        c.name = course.Name;
                        c.startHour = course.StartHour;
                        c.day = course.Day;
                        c.endHour = course.EndHour;
                        c.limit = course.Limit;
                    }
                    // Adding new Course
                    else
                    {
                        data.Courses.Add(CourseConverter.DtoToDataAccess(course));
                    }
                    await data.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> RemoveCourse(int courseId)
        {
            try
            {
                using (var data = Context)
                {
                    var c = await (from item in data.Courses where item.id == courseId select item).FirstAsync();
                    data.Courses.Remove(c);
                    await data.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
