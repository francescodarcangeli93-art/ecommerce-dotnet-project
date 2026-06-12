using BLL_Ecommerce.Interfaces;
using BLL_Ecommerce.Services;
using DTO_Ecommerce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using System.Web.Services.Description;
using WebEcommerce.Models;

namespace WebEcommerce.Controllers
{
    public class GestIndirizziController : Controller
    {
        // GET api/<controller>
        private readonly ILUserService _userservices;
        private readonly ILCustomerService _customerservices;
   

        public GestIndirizziController()
        {
            _userservices = new UserServices();
            _customerservices = new CustomerServices();
      
        }


        // lista indirizzi

        [HttpGet]
        public ActionResult Index(int? id)
        {
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

        [HttpPost]
        public JsonResult AggiungiIndirizzo (sp_ManageCustomerAddress_Param param)
        {
            try
            {
                param.Operation = "I";
                _customerservices.sp_ManageCustomerAddress(param);

                return Json(new { Success = true, Message = "Salvato con successo!"});
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult EliminaIndirizzo(int? id)
        {
            try
            {
                sp_ManageCustomerAddress_Param param = new sp_ManageCustomerAddress_Param
                {
                    Operation = "D",
                    Id = id
                };
               
                _customerservices.sp_ManageCustomerAddress(param);

                return Json(new { Success = true, Message = "Indirizzo eliminato con successo" });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message });
            }
        }

    }
}