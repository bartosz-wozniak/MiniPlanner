using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.DtoObjects;
using DataAccess;

namespace BusinessLogic.Logic
{
    public class SchedulerLogic
    {
        public MiniPlannerDbEntities Context { get; set; }

        public SchedulerLogic()
        {
            Context = new MiniPlannerDbEntities();
        }

        public bool Run()
        {
            try
            {
                Task.Factory.StartNew(Compute);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task Compute()
        {
            List<DtoCourse> allCourses = (await new CourseLogic().GetCourses()).ToList();
            List<DtoUser> notAssignedUsers = new List<DtoUser>();
            List<DtoSchedule> assignedSchedules = new List<DtoSchedule>();
            Dictionary<int, int> freePlaces = (await new CourseLogic().GetCourses()).ToDictionary(item => item.Id, item => 0);
            List<DtoSchedule> allSchedules = await new ScheduleLogic().GetAllSchedules();
            List<DtoUser> users =
                allSchedules.OrderByDescending(item => item.User.AverageScore).GroupBy(item => item.User).Select(item => item.Key).Distinct().ToList();
            foreach (DtoUser user in users)
            {
                List<int> schedules =
                    allSchedules.Where(item => item.User.Id == user.Id)
                        .GroupBy(item => item.ScheduleId)
                        .OrderBy(item => item.Key)
                        .Select(item => item.Key)
                        .ToList();
                bool success = false;
                foreach (int schedule in schedules)
                {
                    List<DtoCourse> courses =
                        allSchedules.Where(item => item.User.Id == user.Id && item.ScheduleId == schedule)
                            .OrderBy(item => item.Course.Id)
                            .Select(item => item.Course)
                            .ToList();
                    bool canAdd = true;
                    foreach (DtoCourse course in courses)
                    {
                        if (freePlaces[course.Id] >= course.Limit)
                        {
                            canAdd = false;
                        }
                    }
                    if (canAdd)
                    {
                        foreach (var course in courses)
                        {
                            assignedSchedules.Add(new DtoSchedule { Course = course, ScheduleId = schedule * 1000, User = user });
                            freePlaces[course.Id]++;
                        }
                        success = true;
                        break;
                    }
                }
                if (!success)
                    notAssignedUsers.Add(user);
            }
            if (notAssignedUsers.Any())
            {
                foreach (DtoUser user in notAssignedUsers)
                {
                    int minConflicts = int.MaxValue;
                    int bestSchedule = 0;
                    List<DtoCourse> bestCoursesToChange = new List<DtoCourse>();
                    List<int> schedules =
                        allSchedules.Where(item => item.User.Id == user.Id)
                            .GroupBy(item => item.ScheduleId)
                            .OrderBy(item => item.Key)
                            .Select(item => item.Key)
                            .ToList();
                    foreach (int schedule in schedules)
                    {
                        List<List<DtoCourse>> coursesToAssignRandomly = new List<List<DtoCourse>>();
                        int conflictsCounter = 0;
                        List<DtoCourse> courses =
                            allSchedules.Where(item => item.User.Id == user.Id && item.ScheduleId == schedule)
                                .OrderBy(item => item.Course.Id)
                                .Select(item => item.Course)
                                .ToList();
                        List<DtoCourse> originalCourses = courses.ToList();
                        foreach (DtoCourse course in courses.ToList())
                        {
                            //if (freePlaces[course.Id] >= course.Limit)
                            {
                                //conflictsCounter++;
                                coursesToAssignRandomly.Add(allCourses.Where(item => item.Name == course.Name && freePlaces[item.Id] < item.Limit).ToList());
                                courses.Remove(course);
                            }
                        }
                        bool shouldBreak = false;
                        for (int i = 0; i < coursesToAssignRandomly.Count; ++i)
                        {
                            if (shouldBreak) break;

                            for (int j = 0; j < coursesToAssignRandomly[i].Count; ++j)
                            {
                                courses.Add(coursesToAssignRandomly[i][j]);
                                if (i + 1 < coursesToAssignRandomly.Count)
                                {
                                    i++;
                                    j = -1;
                                }
                                else
                                {
                                    for (int k = 0; k < courses.Count - 1; ++k)
                                    {
                                        var course1 = courses[k];
                                        for (int l = k + 1; l < courses.Count; ++l)
                                        {
                                            var course2 = courses[l];
                                            if ((course1.Day == course2.Day) &&
                                                ((course1.StartHour >= course2.StartHour && course1.StartHour < course2.EndHour) ||
                                                (course1.EndHour > course2.StartHour && course1.EndHour <= course2.EndHour) ||
                                                (course1.StartHour <= course2.StartHour && course1.EndHour >= course2.EndHour)))
                                            {
                                                conflictsCounter += 20;
                                            }
                                        }
                                    }
                                    conflictsCounter += originalCourses.Count(originalCourse => courses.All(item => item.Id != originalCourse.Id));
                                    if (conflictsCounter < minConflicts)
                                    {
                                        minConflicts = conflictsCounter;
                                        bestSchedule = schedule;
                                        bestCoursesToChange = new List<DtoCourse>();
                                        for (int k = 0; k < coursesToAssignRandomly.Count; ++k)
                                        {
                                            bestCoursesToChange.Add(courses[courses.Count - 1 - k]);
                                        }
                                    }
                                    for (int k = 0; k < courses.Count - 1; ++k)
                                    {
                                        var course1 = courses[k];
                                        for (int l = k + 1; l < courses.Count; ++l)
                                        {
                                            var course2 = courses[l];
                                            if (course1.Day == course2.Day &&
                                                (course1.StartHour >= course2.StartHour && course1.StartHour < course2.EndHour ||
                                                course1.EndHour > course2.StartHour && course1.EndHour <= course2.EndHour ||
                                                course1.StartHour <= course2.StartHour && course1.EndHour >= course2.EndHour))
                                            {
                                                conflictsCounter -= 20;
                                            }
                                        }
                                    }
                                    conflictsCounter -= originalCourses.Count(originalCourse => courses.All(item => item.Id != originalCourse.Id));
                                    courses.RemoveAt(courses.Count - 1);
                                    if (j == coursesToAssignRandomly[i].Count - 1)
                                    {
                                    label:
                                        DtoCourse tempCourse = new DtoCourse();
                                        if (courses.Count > 0)
                                        {
                                            tempCourse = courses[courses.Count - 1];
                                            courses.RemoveAt(courses.Count - 1);
                                        }
                                        if (i - 1 >= 0)
                                        {
                                            i--;
                                            j = coursesToAssignRandomly[i].IndexOf(coursesToAssignRandomly[i].Find(item => item.Id == tempCourse.Id));
                                            if (j == coursesToAssignRandomly[i].Count - 1)
                                            {
                                                goto label;
                                            }
                                        }
                                        else
                                        {
                                            shouldBreak = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    List<DtoCourse> bestCourses =
                        allSchedules.Where(item => item.User.Id == user.Id && item.ScheduleId == bestSchedule)
                        .OrderBy(item => item.Course.Id)
                        .Select(item => item.Course).Where(item => bestCoursesToChange.All(item2 => item2.Name != item.Name))
                        .ToList();
                    bestCourses.AddRange(bestCoursesToChange);
                    foreach (DtoCourse course in bestCourses)
                    {
                        if (freePlaces[course.Id] < course.Limit)
                        {
                            assignedSchedules.Add(new DtoSchedule { Course = course, ScheduleId = bestSchedule * 1000 + minConflicts, User = user });
                            freePlaces[course.Id]++;
                        }
                        else
                        {
                            throw new ApplicationException();
                        }
                    }
                }
            }
            await new ScheduleLogic().RemoveAllSchedules();
            await new ScheduleLogic().SaveSchedule(assignedSchedules);
            await new RegistrationLogic().UpdateStatus("Zakończona");
        }
    }
}
