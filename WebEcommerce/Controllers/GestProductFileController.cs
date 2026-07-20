using BLL_Ecommerce.Helper.Imaging;
using BLL_Ecommerce.Interfaaces;
using BLL_Ecommerce.Interfaces;
using BLL_Ecommerce.Services;
using DTO_Ecommerce;
using System;
using System.Collections.Generic;
using System.IO;
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

        /*============================================================================================================================
           3  ACTION CHE APRE LA PAGINA DEL DETTAGLIO DEI PRODOTTI
        ==============================================================================================================================*/

        // TO DO: implementare il modo che devo mostrare anche le immagini inattive con il parametro IncludeInactive che di defoult
        // è impostato a false

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
          4 ACTION CHE RESTITUISCE LE IMMAGINI CICLATE ALLA VIEW
        ==========================================================================================================================*/

        [HttpGet]
        public FileResult GetProductFileContent(int id)
        {
            if (id <= 0)
            {
                string defaultImagePath = Server.MapPath("~/Content/Images/no-image.png");
                return File(defaultImagePath, "image/png");
            }

            sp_GetProductFileContent_Result file = _Products.GetProductFileContent(id);



            string mimeType = !string.IsNullOrEmpty(file.FileType) && file.FileType.Contains("/")
                ? file.FileType
                : "application/octet-stream";

            return File(file.FileContent, mimeType, file.FileName);
        }


        /*==================================================================
           5  ACTION CHE RITORNA UNA PARTIAL VIEW PER LA TABELLA.
        ====================================================================*/
        [HttpGet]
        public ActionResult GetProductImagesPartial(int ProductId)
        {
            int idUser = 0;
            try
            {
                if (Session["UserId"] != null) idUser = (int)Session["UserId"];

                ProductCategoryViewModel model = new ProductCategoryViewModel
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
          6  ACTION CHE SI OCCUPA DELL'UPLOAD DELLE IMMAGINI 
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

                // creazione di un array di byte da usare come contenitore per poter elaborare i dati
                byte[] fileBytes;
               
                // in
                using (MemoryStream ms = new System.IO.MemoryStream())
                {   
                    // leggiammo il flusso di dati grezzi e poi vengono copianti nella memoria RAM dentro il MemoryStream
                    uploadFiles.InputStream.CopyTo(ms);

                    fileBytes = ms.ToArray(); // converte il flusso di dati in un array di byte  e lo salviamo ntro l'array di byte fileBytes
                }


                // verificare se l'array di byte è un immagine
                if(uploadFiles.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
                {
                    byte[] editedImage = ImageHelper.CropAndResize(fileBytes, 394, 394);

                    param.FileContent = editedImage;
                    param.FileSizeBytes = editedImage.Length;
                    param.FileType = "Image";
                }
                else
                {
                    param.FileContent = fileBytes;
                    param.FileSizeBytes = uploadFiles.ContentLength;
                    param.FileType = "Manual";
                }

                param.Operation = "I";
                param.FileName = System.IO.Path.GetFileName(uploadFiles.FileName);
                param.FileExtension = System.IO.Path.GetExtension(uploadFiles.FileName);
                param.MimeType = uploadFiles.ContentType;
         

                _Products.ManageProductFile(param);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }

        /*======================================================================================================================
          7  ACTION CHE SI OCCUPA DEL DETTAGLIO DELLE IMMAGINI
         ======================================================================================================================*/
        [HttpGet]
        public ActionResult ImageDetails(int id, int productId)
        {
            int idUser = 0;

            try
            {
                idUser = (int)Session["UserId"];

                return Json(new { success = true, data = _Products.sp_GetImageFileById(id, productId) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);

                // 3. Essendo una chiamata AJAX, restituisco l'errore in JSON, non una View!
                return Json(new { success = false, message = "Errore durante il caricamento del dettaglio." }, JsonRequestBehavior.AllowGet);
            }
            
        }

    }
}
