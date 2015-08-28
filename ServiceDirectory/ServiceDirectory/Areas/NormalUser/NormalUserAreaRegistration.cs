using System.Web.Mvc;

namespace ServiceDirectory.Areas.NormalUser
{
    public class NormalUserAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "NormalUser";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "NormalUser_default",
                "NormalUser/{controller}/{action}/{id}",
                new { controller = "Home", action = "Home", id = UrlParameter.Optional }
            );
        }
    }
}
