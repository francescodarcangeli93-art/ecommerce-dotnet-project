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
    public class GestWarehousesController : Controller
    {
        private readonly ILogService _logService;
        private readonly IWarehousesService _warehousesService;
    
        public GestWarehousesController()
        {
            _warehousesService = new WarehouseService();
            _logService = new LogService();
        }


       

        // GET: Supplier
        public  ActionResult Index()
        {
        
            int idUser = 0;

            try
            {
                idUser = (int)Session["UserId"];

                sp_SearchWarehouses_Param param = new sp_SearchWarehouses_Param();

                WarehousesViewModel Model = new WarehousesViewModel
                {
                    WarehousesList =  _warehousesService.SearchWarehouses(param),
                   
                };

                return View(Model);
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return View("Error");
            }
        }

        // metodo per generare una tabella nella partialview
        [HttpPost]
        public PartialViewResult SearchWarehousesPartial(sp_SearchWarehouses_Param param)
        {

            int idUser = 0;
            try
            {
                idUser = (int)Session["UserId"];

                WarehousesViewModel Model = new WarehousesViewModel
                {
                    WarehousesList = _warehousesService.SearchWarehouses(param)
                };

                return PartialView("_WarehousesTable", Model.WarehousesList);
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return PartialView("_WarehousesTable", new List<sp_SearchWarehouses_Result>());
            }
        }

        //// GET: Supplier/Details
        //[HttpGet]
        //public JsonResult  Details(int id)
        //{
            
        //    int idUser = 0;

        //    if (Session["UserId"] == null)
        //    {
        //        return Json(new { success = false, sessionExpired = true }, JsonRequestBehavior.AllowGet);
        //    }

        //    try
        //    {
        //        idUser = (int)Session["UserId"];

        //        sp_GetSupplierById_Result Supplier = await _supplierService.GetSupplierByIdAsync(id);

        //        if (Supplier == null)
        //        {
        //            return Json(new { success = false, message = "Il fornitore corrispondete non è stato trovato"}, JsonRequestBehavior.AllowGet);
                   
        //        }
        //        return Json(new { success = true, data = Supplier}, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
              
        //        _logService.WriteLogProcessError(idUser, ex.Message);
        //        return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}


        //// POST: Supplier/Create
        //[HttpPost]
        //public async Task<JsonResult> Save(sp_ManageSupplier_Param param)
        //{
        //    int idUser = 0;

        //    if (Session["UserId"] == null)
        //    {
        //        return Json(new { success = false, sessionExpired = true });
        //    }

        //    try
        //    {
        //        idUser = (int)Session["UserId"];
        //        // TODO: Add insert logic here
        //        param.Operation = "I";

        //        await _supplierService.sp_ManageSupplierAsync(param);

        //        return Json(new { success = true });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logService.WriteLogProcessError(idUser, ex.Message);
        //        return Json(new { success = false, message = ex.Message});
        //    }
        //}


        //// GET: Supplier/Edit/5
        //public async Task<JsonResult> Edit(int id)
        //{
        //    int idUser = 0;
        //    try
        //    {
        //        idUser = (int)Session["UserId"];
        //        //  prendere i dati dal database del supplier tramite id
        //    sp_GetSupplierById_Result Supplier = await _supplierService.GetSupplierByIdAsync(id);
        //        // impacchettare i dati e spedirli  alla pagina

        //        return Json(new { success = true, data = Supplier}, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logService.WriteLogProcessError(idUser, ex.Message);
        //        return Json(new { success = false, message = ex.Message });
        //    }
        //}

        //// POST: Supplier/Edit/5
        //[HttpPost]
        //public async Task<JsonResult> Edit(sp_ManageSupplier_Param param)
        //{
        //    int idUser = 0;
        //    try
        //    {
        //        idUser = (int)Session["UserId"];
        //        // TODO: Add update logic here
        //        param.Operation = "U";
        //        await _supplierService.sp_ManageSupplierAsync(param);


        //        return Json(new { success = true });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logService.WriteLogProcessError(idUser, ex.Message);
        //        return Json(new { success = false, message = ex.Message});
        //    }
        //}

       

        //// POST: Supplier/Delete/5
        //[HttpPost]
        //public async Task<JsonResult>Delete(int id)
        //{
        //    int idUser = 0;
        //    try
        //    {
        //        idUser = (int)Session["UserId"];

        //        sp_ManageSupplier_Param param = new sp_ManageSupplier_Param
        //        {
        //            Operation = "D",
        //            Id = id
        //        };

        //        await _supplierService.sp_ManageSupplierAsync(param);

        //        return Json( new { success = true});
        //    }
        //    catch (Exception ex) 
        //    {
        //        _logService.WriteLogProcessError(idUser, ex.Message);
        //        return Json(new { success = false, message = ex.Message });
        //    }
        //}

      

    }
}
