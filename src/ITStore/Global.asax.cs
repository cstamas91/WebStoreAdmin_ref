using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using ITStore.Models;

namespace ITStore
{
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Alkalmazás indulása.
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            Application["guestCount"] = 0; // kinullázzuk a vendégek számát
            Application["userCount"] = 0; // kinullázzuk a felhasználók számát
        }

        /// <summary>
        /// Munkafolyamat indulása.
        /// </summary>
        protected void Session_Start()
        {
            Application["guestCount"] = (Int32)Application["guestCount"] + 1; // egyébként a vendégek 
            Session["ShoppingCart"] = new ShoppingCart();
        }

        /// <summary>
        /// Munkafolyamat vége.
        /// </summary>
        protected void Session_End()
        {
            Application["guestCount"] = (Int32)Application["guestCount"] - 1; // egyébként a vendégek számát
        }

    }
}