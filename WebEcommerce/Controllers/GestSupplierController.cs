using BLL_Ecommerce.Interfaaces;
using BLL_Ecommerce.Interfaces;
using BLL_Ecommerce.Services;
using DTO_Ecommerce;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebEcommerce.Models;
using  WebEcommerce.Filters;


namespace WebEcommerce.Controllers
{
    // tenere ex message solo per lo sviluppo poi  sostituire con un messaggio di errore scritto 

    [CheckSession]
    [CheckAdmin]
    public class GestSupplierController : Controller
    {

        private readonly ISupplierService _supplierService;
        private readonly ILogService _logService;
        private readonly ICountryService _countryService;
        public GestSupplierController ()
        {
            _supplierService = new SupplierService ();
            _logService = new LogService();
            _countryService = new CountryService ();
        }

        // GET: Supplier
        public  ActionResult Index()
        {
           
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
                    SupplierList =  _supplierService.SearchSupplier(param),
                    Country =  _countryService.GetListCountry()
                };

                return View(Model);
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return View("Error");
            }
        }

        // metodo per gernare una tabella nella partialview
        [HttpPost]
        public  PartialViewResult SearchSupplierPartial(sp_SearchSupplier_Param param)
        {

            int idUser = 0;
            try
            {
                idUser = (int)Session["UserId"];

                SupplierViewModel Model = new SupplierViewModel
                {
                    SupplierList = _supplierService.SearchSupplier(param),
                };

                return PartialView("_SupplierTable", Model);
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return PartialView("_SupplierTable", new List<sp_SearchSupplier_Result>());
            }
        }


        // GET: Supplier/Details
        [HttpGet]
        public JsonResult Details(int id)
        {
            
            int idUser = 0;

            if (Session["UserId"] == null)
            {
                return Json(new { success = false, sessionExpired = true }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                idUser = (int)Session["UserId"];

                sp_GetSupplierById_Result Supplier =  _supplierService.GetSupplierById(id);

                if (Supplier == null)
                {
                    return Json(new { success = false, message = "Il fornitore corrispondete non è stato trovato"}, JsonRequestBehavior.AllowGet);
                   
                }
                return Json(new { success = true, data = Supplier}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
              
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        // POST: Supplier/Create
        [HttpPost]
        public JsonResult Save(sp_ManageSupplier_Param param)
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

                 _supplierService.sp_ManageSupplier(param);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false, message = ex.Message});
            }
        }


        // GET: Supplier/Edit/5
        public JsonResult Edit(int id)
        {
            int idUser = 0;
            try
            {
                idUser = (int)Session["UserId"];
                //  prendere i dati dal database del supplier tramite id
            sp_GetSupplierById_Result Supplier =  _supplierService.GetSupplierById(id);
                // impacchettare i dati e spedirli  alla pagina

                return Json(new { success = true, data = Supplier}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Supplier/Edit/5
        [HttpPost]
        public JsonResult Edit(sp_ManageSupplier_Param param)
        {
            int idUser = 0;
            try
            {
                idUser = (int)Session["UserId"];
                // TODO: Add update logic here
                param.Operation = "U";
                 _supplierService.sp_ManageSupplier(param);


                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false, message = ex.Message});
            }
        }

       

        // POST: Supplier/Delete/5
        [HttpPost]
        public JsonResult Delete(int id)
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

                 _supplierService.sp_ManageSupplier(param);

                return Json( new { success = true});
            }
            catch (Exception ex) 
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }

      

    }
}
