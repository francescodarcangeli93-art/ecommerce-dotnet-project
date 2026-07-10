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
    public class GestProductCategoryController : Controller
    {
        private readonly ILogService _logService;
        private readonly IProductCategoryService _ProductCategoryService;
    
        public GestProductCategoryController()
        {
            _ProductCategoryService = new ProductCategoryService();
            _logService = new LogService();
        }




        // GET: Wharehouse
        public ActionResult Index()
        {
        
            int idUser = 0;

            try
            {
                idUser = (int)Session["UserId"];

                sp_SearchProductCategory_Param param = new sp_SearchProductCategory_Param();

                ProductCategoryViewModel Model = new ProductCategoryViewModel
                {
                    TheProductCategory = _ProductCategoryService.sp_SearchProductCategory(param),
                   
                };

                return View(Model);
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult SearchProductCategoryPartial(sp_SearchProductCategory_Param param)
        {

            int idUser = 0;
            try
            {
                idUser = (int)Session["UserId"];

                ProductCategoryViewModel Model = new ProductCategoryViewModel
                {

                    TheProductCategory = _ProductCategoryService.sp_SearchProductCategory(param),
                };

                return PartialView("_CategoryProductTable", Model);
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Content("<tr><td colspan='10' class='text-center text-danger'>Errore durante la ricerca</td></tr>");
            }
        }

    }
}
