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

        // GET: PriceList/Edit/5
        public JsonResult Edit(int id)
        {
            int idUser = 0;

            sp_GetPriceListById_Param param = new sp_GetPriceListById_Param
            {
                PriceListId = id,
                Mode = "H"
            };

            try
            {
                idUser = (int)Session["UserId"];
                //  prendere i dati dal database del supplier tramite id
                sp_GetPriceListById_Result PriceList = _priceListService.GetPriceListById(param);
                // impacchettare i dati e spedirli  alla pagina

                return Json(new { success = true, data = PriceList}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: PriceList/Edit/5
        [HttpPost]
        public JsonResult Edit(sp_ManagePriceList_Param param)
        {
            int idUser = 0;
            try
            {
                idUser = (int)Session["UserId"];
              
                param.Operation = "U";
                _priceListService.ManagePriceList(param);


                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult GetDetailsModal(int id)
        {
            int idUser = 0;

            sp_GetPriceListById_Param param = new sp_GetPriceListById_Param
            {
                PriceListId = id,
                Mode = "B"
            };

            try
            {
                if (Session["UserId"] != null)
                {
                    idUser = (int)Session["UserId"];
                }

                sp_GetPriceListById_Result thePriceList = _priceListService.GetPriceListById(param);

                PriceListViewModel model = new PriceListViewModel
                {
                    Header = thePriceList.Header,
                    Details = thePriceList.Details,
                };

                // 1. Restituisce la PartialView anziché la View completa
                return PartialView("_Details", model);
               
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);

               
                return new HttpStatusCodeResult(500, "Si è verificato un errore durante il caricamento del dettaglio listino.");
            }
        }

        [HttpGet]
        public ActionResult GetItemsTablePartial(int id)
        {
            int idUser = 0;
            try
            {
                var param = new sp_GetPriceListById_Param { PriceListId = id, Mode = "B" };
                var thePriceList = _priceListService.GetPriceListById(param);

                var model = new PriceListViewModel
                {
                    Header = thePriceList.Header,
                    Details = thePriceList.Details
                };

                return PartialView("_ItemsTable", model);
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return new HttpStatusCodeResult(500);
            }
        }

        [HttpPost]
        public JsonResult AddItem(sp_ManagePriceListItem_Param param)
        {
            int idUser = 0;

            try
            {
                idUser = (int)Session["UserId"];
                // TODO: Add insert logic here
                param.Operation = "I";

                _priceListService.ManagePriceListItem(param);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpPost]
        public JsonResult DeleteItem(int id)
        {
            int idUser = 0;
            try
            {
                idUser = (int)Session["UserId"];

                sp_ManagePriceListItem_Param param = new sp_ManagePriceListItem_Param
                {
                    Operation = "D",
                    Id = id
                };

                _priceListService.ManagePriceListItem(param);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: PriceList/Edit/5
        public JsonResult EditItem(int? id)
        {
            int idUser = 0;


            try
            {
                idUser = (int)Session["UserId"];
                //  prendere i dati dal database del supplier tramite id
                sp_GetPriceListItemById_Result PriceList = _priceListService.GetPriceListItemById(id);
                // impacchettare i dati e spedirli  alla pagina

                return Json(new { success = true, data = PriceList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: PriceList/Edit/5
        [HttpPost]
        public JsonResult EditItem(sp_ManagePriceListItem_Param param)
        {
            int idUser = 0;
            try
            {
                idUser = (int)Session["UserId"];

                param.Operation = "U";
                _priceListService.ManagePriceListItem(param);


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
