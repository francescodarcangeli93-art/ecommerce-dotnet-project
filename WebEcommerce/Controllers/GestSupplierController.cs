using BLL_Ecommerce.Interfaaces;
using BLL_Ecommerce.Interfaces;
using BLL_Ecommerce.Services;
using DTO_Ecommerce;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Services.Description;
using WebEcommerce.Models;

namespace WebEcommerce.Controllers
{
    // tenere ex message solo per lo sviluppo poi  sostituire con un messaggio di errore scritto 
    public class GestSupplierController : Controller
    {

        private readonly ISupplierService _supplierService;
        private readonly ILogService _logService;
        public GestSupplierController ()
        {
            _supplierService = new SupplierService ();
            _logService = new LogService();
        }

        // GET: Supplier
        public async Task<ActionResult> Index()
        {
            if (Session["Role"] == null || Session["Role"].ToString().ToLower() != "admin")
            {
                return View("Error");
            }

            int idUser = 0;

            try
            {
                idUser = (int)Session["UserId"];

                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Index", "Login");
                }

                sp_SearchSupplier_Param param = new sp_SearchSupplier_Param();

                SupplierViewModel Model = new SupplierViewModel
                {
                    SupplierList = await _supplierService.SearchSupplierAsync(param),
                };

                return View(Model);
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return View("Error");
            }
        }


        // LISTA DEI SUPPLIER
        [HttpPost]
        public async Task<JsonResult> SearchSupplier(sp_SearchSupplier_Param param)
        {
           
            int idUser = 0;

            try
            {
                idUser = (int)Session["UserId"];

                List<sp_SearchSupplier_Result> risultati = await _supplierService.SearchSupplierAsync(param);

                return Json(risultati);
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { Success = false, Message = ex.Message });
            }
        }


        // GET: Supplier/Details
        [HttpGet]
        public async Task <JsonResult> Details(int id)
        {
            
            int idUser = 0;

            if (Session["UserId"] == null)
            {
                return Json(new { success = false, sessionExpired = true }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                idUser = (int)Session["UserId"];

                sp_GetSupplierById_Result Supplier = await _supplierService.GetSupplierByIdAsync(id);

                if (Supplier == null)
                {
                    return Json(new { success = false, Message = "Il fornitore corrispondete non è stato trovato"}, JsonRequestBehavior.AllowGet);
                   
                }
                return Json(new { success = true, data = Supplier}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
              
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        // POST: Supplier/Create
        [HttpPost]
        public async Task<JsonResult> Save(sp_ManageSupplier_Param param)
        {
            int idUser = 0;

            if (Session["UserId"] == null)
            {
                return Json(new { success = false, sessionExpired = true });
            }

            try
            {
                idUser = (int)Session["UserId"];
                // TODO: Add insert logic here
                param.Operation = "I";

                await _supplierService.sp_ManageSupplierAsync(param);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false, ex.Message});
            }
        }


        // GET: Supplier/Edit/5
        public async Task<JsonResult> Edit(int id)
        {
            int idUser = 0;
            try
            {
                idUser = (int)Session["UserId"];
                //  prendere i dati dal database del supplier tramite id
            sp_GetSupplierById_Result Supplier = await _supplierService.GetSupplierByIdAsync(id);
                // impaccettare i dati e spedirli  alla pagina

                return Json(new { success = true, Supplier}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false, ex.Message });
            }
        }

        // POST: Supplier/Edit/5
        [HttpPost]
        public async Task<JsonResult> Edit(sp_ManageSupplier_Param param)
        {
            int idUser = 0;
            try
            {
                idUser = (int)Session["UserId"];
                // TODO: Add update logic here
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false, ex.Message});
            }
        }

       

        // POST: Supplier/Delete/5
        [HttpPost]
        public async Task<JsonResult>Delete(int id)
        {
            int idUser = 0;
            try
            {
                idUser = (int)Session["UserId"];

                sp_ManageSupplier_Param param = new sp_ManageSupplier_Param
                {
                    Operation = "D",
                    Id = id
                };

                await _supplierService.sp_ManageSupplierAsync(param);

                return Json( new { success = true});
            }
            catch (Exception ex) 
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false, Message = ex.Message });
            }
        }

      

    }
}
