using System.Threading.Tasks;
using System.Web.Mvc;
using BusinessLogic.Logic;
using Web.Models;

namespace Web.Controllers
{
    public class RegistrationController : Controller
    {
        public async Task<ActionResult> Registration()
        {
            if (!(await new UserLogic().GetUser(System.Web.HttpContext.Current.User.Identity.Name)).IsAdmin)
                return View("ErrorView");
            return View(await GetStatus());
        }

        private async Task<RegistrationViewModel> GetStatus(string e = "")
        {

            var status = await new RegistrationLogic().GetStatus();
            return new RegistrationViewModel { Status = status, ErrorText = e };
        }

        public async Task<ActionResult> ChangeStatus(string status)
        {
            if (status != "Otwarta" && status != "Zakończona")
            {
                return View("Registration", await GetStatus("Podany status jest niepoprawny. "));
            }
            if (!(await new UserLogic().GetUser(System.Web.HttpContext.Current.User.Identity.Name)).IsAdmin)
                return View("ErrorView");
            var oldStatus = await new RegistrationLogic().GetStatus();
            if (oldStatus == "W trakcie tworzenia planu")
                return View("Registration", await GetStatus("Nie można obecnie zmienić statusu. "));
            await new RegistrationLogic().UpdateStatus(status);
            return View("Registration", await GetStatus());
        }

        public async Task<ActionResult> RunScheduler()
        {
            if (!(await new UserLogic().GetUser(System.Web.HttpContext.Current.User.Identity.Name)).IsAdmin)
                return View("ErrorView");
            var oldStatus = await new RegistrationLogic().GetStatus();
            if (oldStatus == "W trakcie tworzenia planu")
                return View("Registration", await GetStatus("Przydzielanie studentów jest obecnie w trakcie. "));
            await new RegistrationLogic().UpdateStatus("W trakcie tworzenia planu");
#pragma warning disable 4014
            new SchedulerLogic().Run();
#pragma warning restore 4014
            return View("Registration", await GetStatus("Rozpoczęto tworzenie planu"));
        }
    }
}