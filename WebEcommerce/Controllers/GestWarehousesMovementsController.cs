using BLL_Ecommerce.Interfaaces;
using BLL_Ecommerce.Interfaces;
using BLL_Ecommerce.Services;
using DTO_Ecommerce;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.ApplicationServices;
using System.Web.Mvc;
using  WebEcommerce.Filters;
using WebEcommerce.Models;


namespace WebEcommerce.Controllers
{
    // tenere ex message solo per lo sviluppo poi  sostituire con un messaggio di errore scritto 

    [CheckSession]
    [CheckAdmin]
    public class GestWarehousesMovementsController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly ILogService _logService;
        private readonly IWarehousesMovementsService _warehousesMovementsService;

        private static int Cont = 0;

        public GestWarehousesMovementsController()
        {
            _roleService = new RoleServices();
            _warehousesMovementsService = new WarehousesMovementsService();
            _logService = new LogService();
        }

        public ActionResult Index()
        {
            int idUser = 0;

            try
            {
                if (Session["UserId"] != null)
                {
                    idUser = (int)Session["UserId"];
                }

                sp_SearchWarehousesMovements_Param param = new sp_SearchWarehousesMovements_Param();
                
                WarehousesViewModel Model = new WarehousesViewModel
                { 
                    TheProductsList       = _roleService.sp_GetListProduct(),
                    TheMovementReasonList = _warehousesMovementsService.sp_GetMovementReasonList(),
                    //TheWarehouseMovements = _warehousesMovementsService.sp_SearchWarehousesMovements(param),
                    sp_GetAllWareHouseList = _warehousesMovementsService.sp_GetAllWareHouseList()
                };

                // Usa View() per includere il Layout, i CSS e la grafica della pagina!
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
        public ActionResult SearchWarehousesMovementsPartial(sp_SearchWarehousesMovements_Param param, bool? isGlobal)
        {
            ++Cont;
        
            Debug.WriteLine($"\n============ Chiamate al controller {Cont} =======================\n");
           

            int idUser = 0;
            try
            {
                idUser = (int)Session["UserId"];

                List<sp_SearchWarehousesMovements_Result> result = _warehousesMovementsService.sp_SearchWarehousesMovements(param);
              
                int totCount = result.Any() ? result[0].TotalCount : 0;
                
                // se totCount il totale delle righe è 50 e il page size è 20-> 50 /10 ->  totalPage = 3 perchè il Ceiling arrotonda per eccesso
                int totalPage = param.PageSize > 0 ? (int)Math.Ceiling(totCount / (double)param.PageSize) : 0;


                WarehousesViewModel Model = new WarehousesViewModel
                {
                    IsGlobal = isGlobal,
                    TheWarehouseMovements = result,
                    TotCount = totCount,
                    TotalPage = totalPage,
                    CurrentPage = param.PageNumber
                };

                return PartialView("_WarehousesMovementsTable", Model);
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Content("<tr><td colspan='10' class='text-center text-danger'>Errore durante la ricerca</td></tr>");
            }
        }

        [HttpPost]
        public JsonResult SaveMovement(sp_WarehouseMoveProducts_Param param)
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
               

                _warehousesMovementsService.sp_WarehouseMoveProducts(param);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public JsonResult Details (int MovementId)
        {
            Cont = 0;
            int idUser = 0;
            try
            {
                idUser = (int)Session["UserId"];

                WarehousesViewModel Model = new WarehousesViewModel
                {
                    TheGetWarehouseMovementDetail = _warehousesMovementsService.sp_GetWarehousesMovementsById(MovementId),
                };

                return Json(new { success = true, data = Model }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public JsonResult UpdateMovement(int MovementId)
        {
            Cont = 0;
            int idUser = 0;
            try
            {
                idUser = (int)Session["UserId"];

                // 
                string notes = _warehousesMovementsService.sp_GetWarehousesMovementsById(MovementId).Notes;

                return Json(new { success = true, data = notes }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult UpdateMovement(sp_UpdateWarehouseMovementNotes_Param param)
        {
            Cont = 0;
            int idUser = 0;
            try
            {
                idUser = (int)Session["UserId"];

                // richiamare il metodo per salvare i dati di input 

                _warehousesMovementsService.sp_UpdateWarehouseMovementNotes(param);

                return Json(new { success = true});
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
