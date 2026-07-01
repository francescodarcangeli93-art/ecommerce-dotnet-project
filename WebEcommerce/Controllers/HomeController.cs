
using BLL_Ecommerce.Interfaaces;
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
using WebEcommerce.Filters;
using WebEcommerce.Models;

namespace WebEcommerce.Controllers
    {

    [CheckSession]
    [CheckAdmin]
    public class HomeController : Controller
        {

            private readonly ILogService _logService;
            // Azione che gestisce la home

            public HomeController()
            {
               
                _logService = new LogService();
            }

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
                 int idUser = 0;
                try
                {
                    if(Session["UserId"] != null)
                    {
                         idUser = (int)Session["UserId"];

                        _logService.WriteLogLogout(idUser);
                    }
                }
                catch (Exception ex)
                {
                    _logService.WriteLogProcessError(idUser, ex.Message);
                    return Json(new { success = false });
            }
               

                Session.Clear();
                Session.Abandon();
            
                return RedirectToAction("Index", "Login");
            }
    }
    
}
