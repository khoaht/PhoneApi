using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Infrastructure;
using Infrastructure.Domain;
using Infrastructure.Services;

namespace TeleGoApi.Controllers
{
    public class HomeController : Controller
    {

         private readonly IUserService userService;
        private readonly ICustomerCoordinateService customerCoordinateService;

        public HomeController(IUserService userService, ICustomerCoordinateService customerCoordinateService)
        {
            this.customerCoordinateService = customerCoordinateService;
            this.userService = userService;
        }
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
