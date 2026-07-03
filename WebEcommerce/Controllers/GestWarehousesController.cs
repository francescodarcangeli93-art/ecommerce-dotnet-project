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
        [HttpGet]
        public ActionResult SearchWarehousesPartial(sp_SearchWarehouses_Param param)
        {

            int idUser = 0;
            try
            {
                idUser = (int)Session["UserId"];

                WarehousesViewModel Model = new WarehousesViewModel
                {
                    WarehousesList = _warehousesService.SearchWarehouses(param)
                };

                return PartialView("_WarehousesTable", Model);
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Content("<tr><td colspan='10' class='text-center text-danger'>Errore durante la ricerca</td></tr>");
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            int idUser = 0;


            try
            {
                idUser = (int)Session["UserId"];



                WarehousesViewModel Model = new WarehousesViewModel
                {
                    TheWarehouse = _warehousesService.GetWarehouseById(id),
                    TheWarehouseMovementsList = _warehousesService.sp_GetWarehouseMovementsById(id)
                };
                return View(Model);

            }
            catch(Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return View("Error");
            }

        }


        // POST: Supplier/Create
        [HttpPost]
        public JsonResult Save(sp_ManageWarehouse_Param param)
        {
            int idUser = 0;

            try
            {
                idUser = (int)Session["UserId"];
                // TODO: Add insert logic here
                param.Operation = "I";

                _warehousesService.sp_ManageWarehouses(param);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false, message = ex.Message });
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

                sp_ManageWarehouse_Param param = new sp_ManageWarehouse_Param
                {
                    Operation = "D",
                    Id = id
                };

                _warehousesService.sp_ManageWarehouses(param);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false, message = ex.Message });
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
                sp_GetWarehouseById_Result Warehouse = _warehousesService.GetWarehouseById(id);
                // impacchettare i dati e spedirli  alla pagina

                return Json(new { success = true, data = Warehouse }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Supplier/Edit/5
        [HttpPost]
        public JsonResult Edit(sp_ManageWarehouse_Param param)
        {
            int idUser = 0;
            try
            {
                idUser = (int)Session["UserId"];
                // TODO: Add update logic here
                param.Operation = "U";
                _warehousesService.sp_ManageWarehouses(param);


                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}
