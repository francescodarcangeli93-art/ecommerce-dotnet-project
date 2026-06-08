
using BLL_Ecommerce.Services;
using DTO_Ecommerce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.ApplicationServices;
using System.Web.Mvc;
using System.Xml.Linq;
using WebEcommerce.Models;

namespace WebEcommerce.Controllers
    {
        public class HomeController : Controller
        {
            // Azione che gestisce la home
            public ActionResult Index()
            {

                if (Session["User"] == null)
                {
                    return View("Error");
                }

                return View();
            }

            public ActionResult About()
            {
                ViewBag.Message = "Your application description page.";

                return View();
            }

            public ActionResult Contact()
            {
                ViewBag.Message = "Your contact page.";

                return View();
            }


            // azione che  gestisce il logout
            public ActionResult Logout()
            {
                
                Session.Clear();
                Session.Abandon();
            
                return RedirectToAction("Index", "Login");
            }
    }
    
}
