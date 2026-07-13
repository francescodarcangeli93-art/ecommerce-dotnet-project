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
    public class GestProductFileController : Controller
    {
        private readonly ILogService _logService;
        private readonly IProductService _Products;
    
        public GestProductFileController()
        {
            _Products = new ProductService();
            _logService = new LogService();
        }




        /*===================================================================
          1 LISTA PRODOTTI
         ====================================================================*/
        public ActionResult Index()
        {
        
            int idUser = 0;

            try
            {
                idUser = (int)Session["UserId"];

                sp_SearchProduct_Param param = new sp_SearchProduct_Param();

                ProductCategoryViewModel Model = new ProductCategoryViewModel
                {
                    TheProducts = _Products.sp_SearchProduct(param)

                };

                return View(Model);
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return View("Error");
            }
        }


        /*==================================================================
           2  ACTION CHE RITORNA UNA PARTIAL VIEW PER LA TABELLA.
        ====================================================================*/


        [HttpGet]
        public ActionResult SearchProductsPartial(sp_SearchProduct_Param param)
        {

            int idUser = 0;
            try
            {
                idUser = (int)Session["UserId"];

                ProductCategoryViewModel Model = new ProductCategoryViewModel
                {
                    TheProducts = _Products.sp_SearchProduct(param)
                };

                return PartialView("_ProductsTable", Model);
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Content("<tr><td colspan='10' class='text-center text-danger'>Errore durante la ricerca</td></tr>");
            }
        }

        //[HttpGet]
        //public ActionResult Details(int id)
        //{
        //    int idUser = 0;


        //    try
        //    {
        //        idUser = (int)Session["UserId"];



        //        WarehousesViewModel Model = new WarehousesViewModel
        //        {
        //            TheWarehouse = _warehousesService.GetWarehouseById(id),
        //            TheWarehouseMovementsList = _warehousesService.sp_GetWarehouseMovementsById(id)
        //        };
        //        return View(Model);

        //    }
        //    catch (Exception ex)
        //    {
        //        _logService.WriteLogProcessError(idUser, ex.Message);
        //        return View("Error");
        //    }

        //}


    }
}
