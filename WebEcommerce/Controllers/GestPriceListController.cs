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
    public class GestPriceListController : Controller
    {

 
        private readonly ILogService _logService;
        private readonly IPriceListService _priceListService;
 
        public GestPriceListController()
        {
            _logService = new LogService();
            _priceListService = new PriceListService();
         
        }

        
        public  ActionResult Index()
        {
           
            int idUser = 0;

            try
            {
                idUser = (int)Session["UserId"];

                
               
                sp_SearchPriceList_Param param = new sp_SearchPriceList_Param();

                PriceListViewModel Model = new PriceListViewModel
                {
                    ThePriceList = _priceListService.SearchPriceList(param)

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
        public PartialViewResult SearchPriceListPartial(sp_SearchPriceList_Param param)
        {

            int idUser = 0;
            try
            {
                idUser = (int)Session["UserId"];

                PriceListViewModel Model = new PriceListViewModel
                {
                    ThePriceList = _priceListService.SearchPriceList(param)

                };

                return PartialView("_PriceListTable", Model);
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return PartialView("_PriceListTable", new List<sp_SearchPriceList_Result>());
            }
        }

        [HttpPost]
        public JsonResult Save(sp_ManagePriceList_Param param)
        {
            int idUser = 0;

            try
            {
                idUser = (int)Session["UserId"];
                // TODO: Add insert logic here
                param.Operation = "I";

                _priceListService.ManagePriceList(param);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            int idUser = 0;
            try
            {
                idUser = (int)Session["UserId"];

                sp_ManagePriceList_Param param = new sp_ManagePriceList_Param
                {
                    Operation = "D",
                    Id = id
                };

                _priceListService.ManagePriceList(param);

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
