using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Converters;
using BusinessLogic.DtoObjects;
using DataAccess;

namespace BusinessLogic.Logic
{
    public class ScheduleLogic
    {
        public MiniPlannerDbEntities Context { get; set; }

        public ScheduleLogic()
        {
            Context = new MiniPlannerDbEntities();
        }

        public async Task<ObservableCollection<DtoSchedule>> GetSchedules(int filter = -1)
        {
            var ret = new ObservableCollection<DtoSchedule>();
            using (var data = Context)
            {
                if (filter != -1)
                    foreach (var item in await (from item in data.Preferences where item.userId == filter select item).OrderBy(item => item.scheduleId).ToListAsync())
                        ret.Add(ScheduleConverter.DataAccsessToDto(item));
                else
                    foreach (var item in await (from item in data.Preferences select item).OrderBy(item => item.scheduleId).ToListAsync())
                        ret.Add(ScheduleConverter.DataAccsessToDto(item));
                return ret;
            }
        }

        public async Task<List<DtoSchedule>> GetAllSchedules()
        {
            var ret = new List<DtoSchedule>();
            using (var data = Context)
            {
                ret.AddRange(from item in await (from item in data.Preferences select item).OrderBy(item => item.scheduleId).ToListAsync() select ScheduleConverter.DataAccsessToDto(item));
                return ret;
            }
        }

        public async Task<DtoSchedule> GetPreference(int id)
        {
            try
            {
                using (var data = Context)
                    return ScheduleConverter.DataAccsessToDto(await (from item in data.Preferences where item.id == id select item).FirstOrDefaultAsync());
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> SavePreference(DtoSchedule schedule)
        {
            try
            {
                using (var data = Context)
                {
                    var s = await (from item in data.Preferences where schedule.Id == item.id select item).FirstOrDefaultAsync();
                    // Updating Course
                    if (s != null)
                    {
                        s.userId = schedule.User.Id;
                        s.courseId = schedule.Course.Id;
                        s.scheduleId = schedule.Id;
                    }
                    // Adding new Course
                    else
                    {
                        data.Preferences.Add(ScheduleConverter.DtoToDataAccess(schedule));
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

        public async Task<bool> SaveSchedule(IList<DtoSchedule> schedules)
        {
            try
            {
                using (var data = Context)
                {
                    foreach (var item in schedules)
                        data.Preferences.Add(ScheduleConverter.DtoToDataAccess(item));
                    await data.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> RemovePreference(int id)
        {
            try
            {
                using (var data = Context)
                {
                    var s = await (from item in data.Preferences where item.id == id select item).FirstAsync();
                    data.Preferences.Remove(s);
                    await data.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> RemoveSchedule(int userId, int scheduleId)
        {
            try
            {
                using (var data = Context)
                {
                    var s = await (from item in data.Preferences where item.scheduleId == scheduleId && item.userId == userId select item).ToListAsync();
                    var firstOrDefault = s.FirstOrDefault();
                    if (firstOrDefault != null)
                    {
                        foreach (var item in s)
                        {
                            data.Preferences.Remove(item);
                        }
                        if (
                            await
                                (from item in data.Preferences
                                 where item.userId == userId && item.scheduleId > scheduleId
                                 select item).AnyAsync())
                        {
                            s = await
                                (from item in data.Preferences
                                 where item.userId == userId && item.scheduleId > scheduleId
                                 select item).ToListAsync();
                            foreach (var item in s)
                            {
                                item.scheduleId--;
                            }
                        }
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

        public async Task<bool> RemoveAllSchedules()
        {
            try
            {
                using (var data = Context)
                {
                    foreach (var item in await (from item in data.Preferences select item).ToListAsync())
                        data.Preferences.Remove(item);
                    await data.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> MoveUpSchedule(int userId, int scheduleId)
        {
            try
            {
                using (var data = Context)
                {
                    var s =
                        await
                            (from item in data.Preferences
                             where item.scheduleId == scheduleId && item.userId == userId
                             select item).ToListAsync();
                    var s2 =
                        await
                            (from item in data.Preferences
                             where item.scheduleId == scheduleId - 1 && item.userId == userId
                             select item).ToListAsync();
                    if (s.Any() && s2.Any())
                    {
                        foreach (var item in s)
                        {
                            item.scheduleId--;
                        }
                        foreach (var item in s2)
                        {
                            item.scheduleId++;
                        }
                        await data.SaveChangesAsync();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> MoveDownSchedule(int userId, int scheduleId)
        {
            try
            {
                using (var data = Context)
                {
                    var s =
                        await
                            (from item in data.Preferences
                             where item.scheduleId == scheduleId && item.userId == userId
                             select item).ToListAsync();
                    var s2 =
                        await
                            (from item in data.Preferences
                             where item.scheduleId == scheduleId + 1 && item.userId == userId
                             select item).ToListAsync();
                    if (s.Any() && s2.Any())
                    {
                        foreach (var item in s)
                        {
                            item.scheduleId++;
                        }
                        foreach (var item in s2)
                        {
                            item.scheduleId--;
                        }
                        await data.SaveChangesAsync();
                    }
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
