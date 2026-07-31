using BLL_Ecommerce.Interfaaces;
using BLL_Ecommerce.Interfaces;
using BLL_Ecommerce.Services;
using DTO_Ecommerce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
    public class GestWarehousesController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly ILogService _logService;
        private readonly IWarehousesService _warehousesService;
        private readonly IWarehousesMovementsService _warehousesMovementsService;

        public GestWarehousesController()
        {
            _roleService = new RoleServices();
            _warehousesService = new WarehouseService();
            _logService = new LogService();
            _warehousesMovementsService = new WarehousesMovementsService();
        }




        // GET: Wharehouse
        public ActionResult Index()
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
        public ActionResult GetTabellaGiacenze(int warehouseId)
        {

            int idUser = 0;
            try
            {
                idUser = (int)Session["UserId"];

                WarehousesViewModel Model = new WarehousesViewModel
                {
                    TheWarehouseStock = _warehousesService.sp_GetWarehouseStockProductById(warehouseId)
                };

                return PartialView("_TabellaGiacenzePartial", Model);
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
                    TheProductsList = _roleService.sp_GetListProduct(),
                    TheWarehouse = _warehousesService.GetWarehouseById(id),
                    TheWarehouseMovementsList = _warehousesService.sp_GetWarehouseMovementsById(id),
                    TheMovementReasonList = _warehousesMovementsService.sp_GetMovementReasonList(),
                    TheProductsNotInTheWarouseList = getWareHousesProductNotInStock(id)
                    //TODO: verificare ce non ci siano problemi
                    //TheWarehouseStock = _warehousesService.sp_GetWarehouseStockProductById(id)
                };
                return View(Model);

            }
            catch(Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return View("Error");
            }

        }


        // POST: Wharehouse/Create
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

        // POST: Wharehouse/Delete/5
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

        // GET: Wharehouse/Edit/5
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

        // POST: Wharehouse/Edit/5
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

        [HttpPost]
        public JsonResult CaricoProdotti(sp_WarehouseMoveProducts_Param param)
        {
         
            int idUser = 0;


            if (Session["UserId"] == null)
            {
                return Json(new { success = false, sessionExpired = true });
            }

            try
            {
                idUser = (int)Session["UserId"];
        


                _warehousesMovementsService.sp_WarehouseMoveProducts(param);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false, message = ex.Message });
            }

        }


        private List<sp_GetListProduct_Result> getWareHousesProductNotInStock(int warehousesId)
        {   
            // i prodotti che sono in magazzino 
            List<sp_GetWarehouseStockProductById_Result> theStockProductList = _warehousesService.sp_GetWarehouseStockProductById(warehousesId);

            if (theStockProductList == null || theStockProductList.Count == 0)
            {
                return new List<sp_GetListProduct_Result> ();
            }

            // lista di tutti i prodotti del catalogo
            List<sp_GetListProduct_Result> TheProductsList = _roleService.sp_GetListProduct();

            // variabile temporanea che prende tutti i  protti  non sono nel magazino 


           
            // salvo gli id gia in giacenza nel magazzino in un hashset
            HashSet<int> idInStock = theStockProductList.Select(x => x.ProductId).ToHashSet();

            // otteniamo i prodotti che non sono nel magazzino
            List<sp_GetListProduct_Result> noWareHousesProduct = TheProductsList.Where(product => !idInStock.Contains(product.Id)).ToList();

            return noWareHousesProduct;
        }


       
    }
}
