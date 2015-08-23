using System.Web.Mvc;

namespace ServiceDirectory.Areas.SuperUser
{
    public class SuperUserAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SuperUser";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SuperUser_default",
                "SuperUser/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
