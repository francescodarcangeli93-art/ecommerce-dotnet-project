using BLL_Ecommerce.Interfaaces;
using BLL_Ecommerce.Services;
using DAL_Ecommerce;
using DTO_Ecommerce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace WebEcommerce.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginServices _loginService;


        public LoginController()
        {
            _loginService = new LoginService();
        }

        public LoginController(ILoginServices loginService)
        {
            _loginService = loginService;
        }


        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string user, string password)
        {
            // A. Creiamo l'oggetto con i parametri per la Stored Procedure
            sp_LoginUser_Param param = new sp_LoginUser_Param();
            param.Username = user;
            param.Password = password;

            // B. Chiamiamo il LoginService (che cripta la password in automatico)

            //LoginService loginService = new LoginService();
            List<sp_LoginUser_Result> listResult = _loginService.LoginUser(param);

            // C. Facciamo il controllo sul risultato del Database
            if (listResult != null && listResult.Count > 0)
            {
                var utenteLoggato = listResult[0];

                // D. Salviamo i dati in Sessione (così la Navbar li vede!)
                Session["User"] = utenteLoggato.UserName;
                Session["Role"] = utenteLoggato.RoleName;
                Session["UserId"] = utenteLoggato.UserId;
                
                // E. Login corretto! Voliamo sulla Home Page
                return RedirectToAction("Index", "Home");

            }

            // F. Se siamo qui, il login è fallito. Ricarichiamo la pagina con un errore
            ViewData["Errore"] = "credenziali errate";
            return View("Index");
        }


    }
        
    
}