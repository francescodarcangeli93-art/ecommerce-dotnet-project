using BLL_Ecommerce.Interfaaces;
using BLL_Ecommerce.Interfaces;
using BLL_Ecommerce.Services;
using DTO_Ecommerce;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WebEcommerce.Models;

namespace WebEcommerce.Controllers
{

    public class GestIndirizziController : Controller
    {

        // GET api/<controller>
        private readonly IUserService _userservices;
        private readonly ICustomerService _customerservices;
        private readonly ILogService _logService;

        public GestIndirizziController()
        {
            _userservices = new UserServices();
            _customerservices = new CustomerServices();
            _logService = new LogService();
        }
        
        [HttpGet]
        public ActionResult Index(int? id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            int idUser = 0;

            try
            {
                idUser = (int)Session["UserId"];
                //  recupero dell'utente tramite l'id
                sp_GetUserById_Result User = _userservices.GetUserById(id);

                int? customerId = User.CustomerId;

                // recupero degli indirizzi tramite la search
                List<sp_GetCustomerAddresses_Result> CustomerAddressess = _customerservices.GetCustomerAddresses(customerId);


                // mettere i dati nel model
                GestioneUtentiViewModel Model = new GestioneUtentiViewModel
                {
                    UserDetail = User,
                    CustomerAddresses = CustomerAddressess
                };

                return View(Model);
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return View("Error");
            }
        }

        [HttpPost]
        public JsonResult AggiungiIndirizzo (sp_ManageCustomerAddress_Param param)
        {
            int idUser = 0;
            try
            {
                idUser = (int)Session["UserId"];
                param.Operation = "I";

                _customerservices.sp_ManageCustomerAddress(param);

                return Json(new { success = true, Message = "Salvato con successo!"});
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false, Message = ex.Message });
            }

        }

        [HttpPost]
        public JsonResult EliminaIndirizzo(sp_ManageCustomerAddress_Param param)
        {
            
            int idUser = 0;
            try
            {
                idUser = (int)Session["UserId"];
                sp_ManageCustomerAddress_Param p = new sp_ManageCustomerAddress_Param()
                {
                    Operation = "D",
                    Id = param.Id
                };

                _customerservices.sp_ManageCustomerAddress(p);

                return Json(new { success = true, Message = "Indirizzo eliminato con successo" });
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false, Message = ex.Message });
            }
        }

        // MODIFICA INDIRIZZO

        [HttpPost]
        public JsonResult ModificaIndirizzo(sp_ManageCustomerAddress_Param param)
        {
            int idUser = 0;

            try
            {
                idUser = (int)Session["UserId"];
                param.Operation = "U";

                _customerservices.sp_ManageCustomerAddress(param);

                return Json(new { success = true, message = "Salvato con successo!"});
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false, Message = ex.Message });
            }
        }

        

    }
}