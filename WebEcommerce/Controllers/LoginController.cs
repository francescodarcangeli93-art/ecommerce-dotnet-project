using BLL_Ecommerce.Interfaaces;
using BLL_Ecommerce.Services;
using DTO_Ecommerce;
using System;
using System.Collections.Generic;
using System.Web.Mvc;


namespace WebEcommerce.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;
        private readonly ILogService _logService;

        public LoginController()
        {
            _loginService = new LoginService();
             _logService = new LogService();
        }

        public LoginController(ILoginService loginService)
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

            if(string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
            {
               
                return View("Index");
            }

            // A. Creiamo l'oggetto con i parametri per la Stored Procedure
            sp_LoginUser_Param param = new sp_LoginUser_Param();
            param.Username = user;
            param.Password = password;

            // B. Chiamiamo il LoginService (che cripta la password in automatico)
            int idUser = 0;

            //LoginService loginService = new LoginService();
            List<sp_LoginUser_Result> listResult = _loginService.LoginUser(param);
            try
            {
                // C. Facciamo il controllo sul risultato del Database
                if (listResult != null && listResult.Count > 0)
                {
                    sp_LoginUser_Result utenteLoggato = listResult[0];
                    idUser = utenteLoggato.UserId;
                    // D. Salviamo i dati in Sessione (così la Navbar li vede!)
                    Session["User"] = utenteLoggato.UserName;
                    Session["Role"] = utenteLoggato.RoleName;
                    Session["UserId"] = utenteLoggato.UserId;

                    _logService.WriteLogLogin(utenteLoggato.UserId);

                    // E. Login corretto! Voliamo sulla Home Page
                    return RedirectToAction("Index", "Home");

                }

                _logService.WriteLogProcessWarning(idUser, "tentativo di Login fallito");
            
                // F. Se siamo qui, il login è fallito. Ricarichiamo la pagina con un errore

                ViewData["Errore"] = "credenziali errate";

                return View("Index");
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false });
            }   
        }

    }
        
    
}