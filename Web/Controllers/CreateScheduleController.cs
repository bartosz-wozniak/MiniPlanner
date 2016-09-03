using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using BusinessLogic.DtoObjects;
using BusinessLogic.Logic;
using Web.Models;

namespace Web.Controllers
{
    public class CreateScheduleController : Controller
    {
        public async Task<ActionResult> CreateSchedule()
        {
            if (await new RegistrationLogic().GetStatus() != "Otwarta")
                return View("OfferedCourses", new OfferedCoursesViewModel { Courses = await new CourseLogic().GetCourses() });
            return View(await GetAllCourses());
        }

        public async Task<ActionResult> SaveSchedule()
        {
            if (await new RegistrationLogic().GetStatus() != "Otwarta")
                return View("OfferedCourses", new OfferedCoursesViewModel { Courses = await new CourseLogic().GetCourses() });
            string error;
            var coursesToChoose = await new CourseLogic().GetCourses();
            var chosenCourses = Session["ChosenCourses"] as List<DtoCourse> ?? new List<DtoCourse>();
            if (chosenCourses.Any())
            {
                if (await new RegistrationLogic().GetStatus() == "Otwarta")
                {
                    var user = await new UserLogic().GetUser(System.Web.HttpContext.Current.User.Identity.Name);
                    var userSchedules = await new ScheduleLogic().GetSchedules(user.Id) as IEnumerable<DtoSchedule>;
                    var newId = 1;
                    if (userSchedules.Any())
                    {
                        newId = userSchedules.OrderByDescending(item => item.ScheduleId).First().ScheduleId + 1;
                    }
                    IList<DtoSchedule> schedule = chosenCourses.Select(item => new DtoSchedule
                    {
                        Course = item,
                        User = user,
                        ScheduleId = newId
                    }).ToList();
                    var ret = await new ScheduleLogic().SaveSchedule(schedule);
                    if (ret)
                    {
                        Session["ChosenCourses"] = null;
                        error = "Plan dodano pomyślnie";
                    }
                    else
                    {
                        error = "Dodawanie planu nie powiodło się";
                    }
                }
                else
                {
                    error = "Rejestracja jest obecnie zamknięta. Nie możesz dodać nowego planu";
                }
            }
            else
            {
                error = "Nie można dodadać pustego planu";
            }
            return View("CreateSchedule",
                    new CreateScheduleViewModel
                    {
                        CoursesChosen = Session["ChosenCourses"] as IEnumerable<DtoCourse>,
                        CoursesToChoose = coursesToChoose,
                        ErrorText = error
                    });
        }

        private async Task<CreateScheduleViewModel> GetAllCourses()
        {
            var coursesToChoose = await new CourseLogic().GetCourses();
            var chosenCourses = Session["ChosenCourses"] as List<DtoCourse> ?? new List<DtoCourse>();
            foreach (var item in coursesToChoose.ToList())
            {
                if (chosenCourses.Any(it => it.Id == item.Id))
                    coursesToChoose.Remove(item);
            }
            return new CreateScheduleViewModel { CoursesToChoose = coursesToChoose, CoursesChosen = chosenCourses };
        }

        public async Task<ActionResult> Delete(int id)
        {
            if (await new RegistrationLogic().GetStatus() != "Otwarta")
                return View("OfferedCourses", new OfferedCoursesViewModel { Courses = await new CourseLogic().GetCourses() });
            var coursesToChoose = await new CourseLogic().GetCourses();
            var chosenCourses = Session["ChosenCourses"] as List<DtoCourse> ?? new List<DtoCourse>();
            var course = chosenCourses.FirstOrDefault(item => item.Id == id);
            chosenCourses.Remove(course);
            Session["ChosenCourses"] = chosenCourses;
            foreach (var item in coursesToChoose.ToList().Where(item => chosenCourses.Any(it => it.Id == item.Id)))
                coursesToChoose.Remove(item);
            return View("CreateSchedule", new CreateScheduleViewModel { CoursesChosen = chosenCourses, CoursesToChoose = coursesToChoose });
        }

        [AllowAnonymous]
        public async Task<ActionResult> Add(int id)
        {
            if (await new RegistrationLogic().GetStatus() != "Otwarta")
                return View("OfferedCourses", new OfferedCoursesViewModel { Courses = await new CourseLogic().GetCourses() });
            string error = "";
            var coursesToChoose = await new CourseLogic().GetCourses();
            List<DtoCourse> chosenCourses;
            if (coursesToChoose.Any(item => item.Id == id))
            {
                var course = coursesToChoose.FirstOrDefault(item => item.Id == id);
                chosenCourses = Session["ChosenCourses"] as List<DtoCourse> ?? new List<DtoCourse>();
                if (chosenCourses.All(item => item.Id != id && item.Name != course?.Name &&
                                              !(item.Day == course?.Day &&
                                                ((item.StartHour >= course?.StartHour && item.StartHour < course.EndHour) ||
                                                 (item.EndHour > course?.StartHour && item.EndHour <= course.EndHour)))))
                    chosenCourses.Add(course);
                else if (chosenCourses.All(item => item.Id != id))
                    error = "Nie można dodać tej grupy, ponieważ koliduje z inną już wybraną";
                chosenCourses = chosenCourses.OrderBy(item => item.Id).ToList();
                Session["ChosenCourses"] = chosenCourses;
                foreach (var item in coursesToChoose.ToList().Where(item => chosenCourses.Any(it => it.Id == item.Id)))
                    coursesToChoose.Remove(item);
                return View("CreateSchedule",
                    new CreateScheduleViewModel
                    {
                        CoursesChosen = chosenCourses,
                        CoursesToChoose = coursesToChoose,
                        ErrorText = error
                    });
            }
            chosenCourses = Session["ChosenCourses"] as List<DtoCourse> ?? new List<DtoCourse>();
            foreach (var item in coursesToChoose.ToList().Where(item => chosenCourses.Any(it => it.Id == item.Id)))
                coursesToChoose.Remove(item);
            return View("CreateSchedule",
                new CreateScheduleViewModel
                {
                    CoursesChosen = chosenCourses,
                    CoursesToChoose = coursesToChoose,
                    ErrorText = error
                });
        }
    }
}