using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using BusinessLogic.DtoObjects;
using BusinessLogic.Logic;
using Web.Models;

namespace Web.Controllers
{
    public class ViewOwnSchedulesController : Controller
    {
        public async Task<ActionResult> ViewOwnSchedules()
        {
            if (await new RegistrationLogic().GetStatus() != "Otwarta")
                return View("RegistrationEnded", new ViewOwnSchedulesViewModel { ScheduleDetails = await new ScheduleLogic().GetSchedules((await new UserLogic().GetUser(System.Web.HttpContext.Current.User.Identity.Name)).Id) });
            return View(await GetUserSchedules());
        }

        private async Task<ViewOwnSchedulesViewModel> GetUserSchedules()
        {
            var user = await new UserLogic().GetUser(System.Web.HttpContext.Current.User.Identity.Name);
            var schedules = await new ScheduleLogic().GetSchedules(user.Id) as IEnumerable<DtoSchedule>;
            var schedulesGrouped = schedules.GroupBy(item => item.ScheduleId).ToList();
            IList<int> ret = schedulesGrouped.Select(item => item.Key).ToList();
            return new ViewOwnSchedulesViewModel { Schedules = ret };
        }

        public async Task<ActionResult> Delete(int id)
        {
            if (await new RegistrationLogic().GetStatus() != "Otwarta")
                return View("RegistrationEnded", new ViewOwnSchedulesViewModel { ScheduleDetails = await new ScheduleLogic().GetSchedules((await new UserLogic().GetUser(System.Web.HttpContext.Current.User.Identity.Name)).Id) });
            var user = await new UserLogic().GetUser(System.Web.HttpContext.Current.User.Identity.Name);
            var errorText = "";
            bool isSuccess = true;
            if (await new RegistrationLogic().GetStatus() == "Otwarta")
            {
                isSuccess = await new ScheduleLogic().RemoveSchedule(user.Id, id);
            }
            else
            {
                errorText = "Rejestracja jest obecnie zamknięta. Nie możesz usunąć planu";
            }
            var schedules = await new ScheduleLogic().GetSchedules(user.Id) as IEnumerable<DtoSchedule>;
            var schedulesGrouped = schedules.GroupBy(item => item.ScheduleId).ToList();
            IList<int> ret = schedulesGrouped.Select(item => item.Key).ToList();
            if (!isSuccess)
            {
                errorText = "Nie można w tej chwili usunąć planu.";
            }
            return View("ViewOwnSchedules", new ViewOwnSchedulesViewModel { Schedules = ret, ErrorText = errorText });
        }

        public async Task<ActionResult> Select(int id)
        {
            if (await new RegistrationLogic().GetStatus() != "Otwarta")
                return View("RegistrationEnded", new ViewOwnSchedulesViewModel { ScheduleDetails = await new ScheduleLogic().GetSchedules((await new UserLogic().GetUser(System.Web.HttpContext.Current.User.Identity.Name)).Id) });
            var user = await new UserLogic().GetUser(System.Web.HttpContext.Current.User.Identity.Name);
            var schedules = await new ScheduleLogic().GetSchedules(user.Id) as IEnumerable<DtoSchedule>;
            var schedulesGrouped = schedules.GroupBy(item => item.ScheduleId).ToList();
            IList<int> ret = schedulesGrouped.Select(item => item.Key).ToList();
            var scheduleDetails = schedules.Where(item => item.ScheduleId == id);
            return View("ViewOwnSchedules", new ViewOwnSchedulesViewModel { Schedules = ret, ScheduleDetails = scheduleDetails });
        }

        public async Task<ActionResult> MoveUp(int id)
        {
            if (await new RegistrationLogic().GetStatus() != "Otwarta")
                return View("RegistrationEnded", new ViewOwnSchedulesViewModel { ScheduleDetails = await new ScheduleLogic().GetSchedules((await new UserLogic().GetUser(System.Web.HttpContext.Current.User.Identity.Name)).Id) });
            var user = await new UserLogic().GetUser(System.Web.HttpContext.Current.User.Identity.Name);
            var errorText = "";
            bool isSuccess = true;
            if (await new RegistrationLogic().GetStatus() == "Otwarta")
            {
                isSuccess = await new ScheduleLogic().MoveUpSchedule(user.Id, id);
            }
            else
            {
                errorText = "Rejestracja jest obecnie zamknięta. Nie możesz zmienić priorytetu planu";
            }
            var schedules = await new ScheduleLogic().GetSchedules(user.Id) as IEnumerable<DtoSchedule>;
            var schedulesGrouped = schedules.GroupBy(item => item.ScheduleId).ToList();
            IList<int> ret = schedulesGrouped.Select(item => item.Key).ToList();
            if (!isSuccess)
            {
                errorText = "Nie można w tej chwili zmienić priorytetu planu.";
            }
            return View("ViewOwnSchedules", new ViewOwnSchedulesViewModel { Schedules = ret, ErrorText = errorText });
        }

        public async Task<ActionResult> MoveDown(int id)
        {
            if (await new RegistrationLogic().GetStatus() != "Otwarta")
                return View("RegistrationEnded", new ViewOwnSchedulesViewModel { ScheduleDetails = await new ScheduleLogic().GetSchedules((await new UserLogic().GetUser(System.Web.HttpContext.Current.User.Identity.Name)).Id) });
            var user = await new UserLogic().GetUser(System.Web.HttpContext.Current.User.Identity.Name);
            var errorText = "";
            bool isSuccess = true;
            if (await new RegistrationLogic().GetStatus() == "Otwarta")
            {
                isSuccess = await new ScheduleLogic().MoveDownSchedule(user.Id, id);
            }
            else
            {
                errorText = "Rejestracja jest obecnie zamknięta. Nie możesz zmienić priorytetu planu";
            }
            var schedules = await new ScheduleLogic().GetSchedules(user.Id) as IEnumerable<DtoSchedule>;
            var schedulesGrouped = schedules.GroupBy(item => item.ScheduleId).ToList();
            IList<int> ret = schedulesGrouped.Select(item => item.Key).ToList();
            if (!isSuccess)
            {
                errorText = "Nie można w tej chwili zmienić priorytetu planu.";
            }
            return View("ViewOwnSchedules", new ViewOwnSchedulesViewModel { Schedules = ret, ErrorText = errorText });
        }
    }
}