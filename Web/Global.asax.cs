using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Web.Controllers;

namespace Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();

            Server.ClearError();

            var routeData = new RouteData();

            routeData.Values.Add("controller", "ErrorPage");

            routeData.Values.Add("action", "Error");

            routeData.Values.Add("exception", exception);

            routeData.Values.Add("statusCode",
                exception.GetType() == typeof (HttpException) ? ((HttpException) exception).GetHttpCode() : 500);

            Response.TrySkipIisCustomErrors = true;

            IController controller = new ErrorPageController();

            controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));

            Response.End();
        }
    }
}
