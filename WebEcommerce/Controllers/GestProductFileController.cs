using BLL_Ecommerce.Interfaaces;
using BLL_Ecommerce.Interfaces;
using BLL_Ecommerce.Services;
using DTO_Ecommerce;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.ApplicationServices;
using System.Web.Mvc;
using  WebEcommerce.Filters;
using WebEcommerce.Models;


namespace WebEcommerce.Controllers
{
    // tenere ex message solo per lo sviluppo poi  sostituire con un messaggio di errore scritto 

    [CheckSession]
    [CheckAdmin]
    public class GestProductFileController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly ILogService _logService;
        private readonly IProductService _Products;
       

        public GestProductFileController()
        {
            _roleService = new RoleServices();
            _Products = new ProductService();
            _logService = new LogService();
        }




        /*===================================================================
          1 LISTA PRODOTTI
         ====================================================================*/
        public ActionResult Index(int? id)
        {
        
            int idUser = 0;

            try
            {
                idUser = (int)Session["UserId"];

                sp_SearchProduct_Param param = new sp_SearchProduct_Param();

                ProductCategoryViewModel Model = new ProductCategoryViewModel
                {
                    TheSupplierList = _roleService.GetListSupplier(),
                    TheProducts = _Products.sp_SearchProduct(param),
                    ListProductCategories = _roleService.ListProductCategory()
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
           2  ACTION CHE RITORNA LA VIEW PER LA TABELLA.
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

        /*==================================================================
           4  ACTION CHE RITORNA LA VIEW PER LA TABELLA.
        ====================================================================*/

        [HttpGet]
        public ActionResult Details(int id, bool IncludeInactive = false)
        {
            int idUser = 0;


            try
            {
                idUser = (int)Session["UserId"];

                List<sp_GetProductFileById_Result> file = _Products.sp_GetProductFileById(id, IncludeInactive);


                ProductCategoryViewModel Model = new ProductCategoryViewModel
                {
                    GetProductFileByIdList = _Products.sp_GetProductFileById(id, IncludeInactive),
                    TheProduct = _Products.sp_GetProductById(id)
                };

                return View(Model);

            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return View("Error");
            }

        }


        /*==========================================================================================================================
         ACTION CHE RESTITUISCE LE IMMAGINI ALLA VIEW
        ==========================================================================================================================*/

        [HttpGet]
        public FileResult GetProductFileContent(int id)
        {
            sp_GetProductFileContent_Result file = _Products.GetProductFileContent(id);

            string mimeType = !string.IsNullOrEmpty(file.FileType) && file.FileType.Contains("/")
                ? file.FileType
                : "application/octet-stream";

            return File(file.FileContent, mimeType, file.FileName);
        }


        /*==================================================================
           3  ACTION CHE RITORNA UNA PARTIAL VIEW PER LA TABELLA.
        ====================================================================*/
        [HttpGet]
        public ActionResult GetProductImagesPartial(int ProductId)
        {
            int idUser = 0;
            try
            {
                if (Session["UserId"] != null) idUser = (int)Session["UserId"];

                var model = new ProductCategoryViewModel
                {
                    GetProductFileByIdList = _Products.sp_GetProductFileById(ProductId, false),
                };

                return PartialView("_Images", model);
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return new HttpStatusCodeResult(500);
            }
        }

        /*======================================================================================================================
          ACTION CHE SI OCCUPA DELL'UPLOAD DELLE IMMAGINI 
         ======================================================================================================================*/
        [HttpPost]
        public JsonResult UploadImage(sp_ManageProductFile_Param param, HttpPostedFileBase uploadFiles)
        {
            int idUser = 0;

            try
            {
                idUser = (int)Session["UserId"];

                if (uploadFiles == null || uploadFiles.ContentLength == 0)
                {
                    return Json(new { success = false, message = "Nessun file selezionato." });
                }

                // Estraiamo i byte del file direttamente dentro il nostro oggetto param
                using (var ms = new System.IO.MemoryStream())
                {
                    uploadFiles.InputStream.CopyTo(ms);
                    param.FileContent = ms.ToArray();
                }

                // TODO: Add insert logic here
                param.Operation = "I";
                param.FileName = System.IO.Path.GetFileName(uploadFiles.FileName);
                param.FileExtension = System.IO.Path.GetExtension(uploadFiles.FileName);
                param.MimeType = uploadFiles.ContentType;
                param.FileSizeBytes = uploadFiles.ContentLength;

                param.FileType = uploadFiles.ContentType.StartsWith("image/") ? "Image" : "Manual";

                _Products.ManageProductFile(param);

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
