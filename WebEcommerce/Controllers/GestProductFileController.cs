using BLL_Ecommerce.Helper.Imaging;
using BLL_Ecommerce.Interfaaces;
using BLL_Ecommerce.Interfaces;
using BLL_Ecommerce.Services;
using DTO_Ecommerce;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using System.Web.ApplicationServices;
using System.Web.Mvc;
using  WebEcommerce.Filters;
using WebEcommerce.Models;
using WebGrease.Activities;


namespace WebEcommerce.Controllers
{
    // tenere ex message solo per lo sviluppo poi  sostituire con un messaggio di errore scritto 

    [CheckSession]
    [CheckAdmin]
    public class GestProductFileController : Controller
    {
        private readonly IValidationService _validationService;
        private readonly IRoleService _roleService;
        private readonly ILogService _logService;
        private readonly IProductService _ProductsService;
       

        public GestProductFileController()
        {
            _validationService = new ValidationService();
            _roleService = new RoleServices();
            _ProductsService = new ProductService();
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
                    TheSupList = _roleService.GetListSupplier(),
                    TheSupplierList = _ProductsService.sp_GetListSupplierProduct(),
                    TheProducts = _ProductsService.sp_SearchProduct(param),
                    ListProductCategories = _ProductsService.sp_GetListProductCategoryProduct()
                };

                return View(Model);
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return View("Error");
            }
        }

        // TO DO: Funzione che carica le immaggini nella tabella 

        //[HttpGet]
        //public FileResult GetImageInProductsTable(int id) 
        //{
        //    int idUser = 0;
        //    try
        //    {

        //        if (Session["UserId"] != null) idUser = (int)Session["UserId"];

        //        sp_GetProductFileContent_Result file = _ProductsService.GetProductFileContent(id);

                  //if (file == null || file.FileContent == null || file.FileContent.Length == 0)
                  //{
                  //  string defaultImagePath = Server.MapPath("~/Content/Images/no-image.png");
                  //  return File(defaultImagePath, "image/png");
                  //}


        //    }
        //    catch (Exception ex)
        //    {
        //        _logService.WriteLogProcessError(idUser, ex.Message);
              
        //    }
        //}

        /*============================================================================================================================
          2  ACTION CHE AGGIUNGE UN NUOVO PRODOTTO
       ==============================================================================================================================*/
        [HttpPost]
        public JsonResult Save(sp_ManageProduct_Param param)
        {
            int idUser = 0;
            try
            {
                idUser = (int)Session["UserId"];

                param.Operation = "I";
                _ProductsService.ManageProduct(param);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }


        /*==================================================================
           3  ACTION CHE RITORNA LA VIEW PER LA TABELLA.
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
                    TheProducts = _ProductsService.sp_SearchProduct(param)
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
          4  ACTION CHE MODIFICA IL PRODOTTO
         ==============================================================================================================================*/

        [HttpGet]
        public JsonResult Update(int id)
        {
            int idUser = 0;
            try
            {
                idUser = (int)Session["UserId"];

                ProductCategoryViewModel Model = new ProductCategoryViewModel
                {

                    TheProduct = _ProductsService.sp_GetProductById(id)
                };

               

                return Json(new { success = true, data = Model}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult Update(sp_ManageProduct_Param param)
        {
            int idUser = 0;
            try
            {
                idUser = (int)Session["UserId"];

                // Imposta l'operazione su 'U' per eseguire l'UPDATE nella Stored Procedure
                param.Operation = "U";

                _ProductsService.ManageProduct(param);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }

        /*============================================================================================================================
                 5  ACTION CHE ELIMINA IL PRODOTTO
        ==============================================================================================================================*/

        [HttpPost]
        public JsonResult Delete(int id)
        {
            int idUser = 0;
            try
            {
                idUser = (int)Session["UserId"];

                // Creiamo il parametro valorizzando solo ciò che serve alla SP per cancellare
                sp_ManageProduct_Param param = new sp_ManageProduct_Param
                {
                    Id = id,
                    Operation = "D" 
                };

                _ProductsService.ManageProduct(param);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }

        /*============================================================================================================================
           6  ACTION CHE APRE LA PAGINA DEL DETTAGLIO DEI PRODOTTI
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

                //List<sp_GetProductFileById_Result> file = _ProductsService.sp_GetProductFileById(id, IncludeInactive, null);


                ProductCategoryViewModel Model = new ProductCategoryViewModel
                {
                   
                    TheProduct = _ProductsService.sp_GetProductById(id)
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
          7 ACTION CHE RESTITUISCE LE IMMAGINI CICLATE ALLA VIEW
        ==========================================================================================================================*/

        [HttpGet]
        public FileResult GetProductFileContent(int id, string isThumb ="M") //S IMMAGINI MOLTO PICCOLE  T IMMAGINI PICCOLE M IMMAIGINI MEDIE   QUALSIASI ALTRO VALORE IMMAGINI ORIGINALE
        {
            int idUser = 0;
            try
            {
                if (Session["UserId"] != null) idUser = (int)Session["UserId"];

                if (id <= 0)
                {
                    string defaultImagePath = Server.MapPath("~/Content/Images/no-image.png");
                    return File(defaultImagePath, "image/png");
                }

                sp_GetProductFileContent_Result file = _ProductsService.GetProductFileContent(id);

                if (file == null || file.FileContent == null || file.FileContent.Length == 0)
                {
                    string defaultImagePath = Server.MapPath("~/Content/Images/no-image.png");
                    return File(defaultImagePath, "image/png");
                }

                string mimeType = !string.IsNullOrEmpty(file.FileType) && file.FileType.Contains("/")
                    ? file.FileType
                    : "application/octet-stream";

                byte[] cutterBytes = file.FileContent;



                //int size = isThumb ? 64 : 412;
                if (isThumb == "S")
                {
                    cutterBytes = ImageHelper.CropAndResize(file.FileContent, 16, 16);
                }
                else if (isThumb == "T") // 
                {
                    cutterBytes = ImageHelper.CropAndResize(file.FileContent, 32, 32);
                }
                else if (isThumb == "M") 
                {
                    cutterBytes = ImageHelper.CropAndResize(file.FileContent, 500, 400);
                }
                
                   
         

                return File(cutterBytes, mimeType, file.FileName);
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                string defaultImagePath = Server.MapPath("~/Content/Images/no-image.png");
                return File(defaultImagePath, "image/png");
            }
        }


        /*==========================================================================================================================
          8 ACTION CHE GESTISCE IL DOWLOAD DEI FILE
        ==========================================================================================================================*/

        [HttpGet]
        public ActionResult DownloadProductDocument(int id)
        {
            int idUser = 0;
            try
            {
                if (Session["UserId"] != null) idUser = (int)Session["UserId"];

                if (id <= 0) return HttpNotFound("ID Documento non valido.");

                // Recupera il file 
                sp_GetProductFileContent_Result file = _ProductsService.GetProductFileContent(id);

                // Controllo se il record o il contenuto binario sono nulli/vuoti
                if (file == null || file.FileContent == null || file.FileContent.Length == 0)
                {
                    return HttpNotFound("Il documento PDF richiesto non è presente nel sistema.");
                }

                // Recupera il nome del file (con un valore di fallback se vuoto nel DB)
                string fileName = !string.IsNullOrEmpty(file.FileName) ? file.FileName : "documento.pdf";

                // controllo del nome che finisce per .pdf
                if (!fileName.ToLower().EndsWith(".pdf"))
                {
                    fileName += ".pdf";
                }

                // Restituisce direttamente il PDF al browser
                return File(file.FileContent, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return new HttpStatusCodeResult(500, "Errore durante il recupero del documento PDF.");
            }
        }

        /*=============================================================================================================================================
          9 ACTION ELIMINA IL FILE
        ===============================================================================================================================================*/

        [HttpPost]
        public JsonResult DeleteProductFile(int id)
        {
            int idUser = 0;
            try
            {
                idUser = (int)Session["UserId"];

                if (id <= 0)
                {
                    return Json(new { success = false, message = "ID File non valido." });
                }

                // Creiamo il parametro valorizzando solo ciò che serve alla SP per cancellare
                sp_ManageProductFile_Param param = new sp_ManageProductFile_Param
                {
                    Id = id,
                    Operation = "D"
                };

                _ProductsService.ManageProductFile(param);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }

     /*========================================================================================================================
        10  ACTION CHE ELIMINA UN IMMAGINE
     ========================================================================================================================*/

        [HttpPost]
        public JsonResult deleteImage(int id)
        {
            int idUser = 0;
            try
            {
                idUser = (int)Session["UserId"];

                if (id <= 0)
                {
                    return Json(new { success = false, message = "ID File non valido." });
                }

                // Creiamo il parametro valorizzando solo ciò che serve alla SP per cancellare
                sp_ManageProductFile_Param param = new sp_ManageProductFile_Param
                {
                    Id = id,
                    Operation = "D"
                };

                _ProductsService.ManageProductFile(param);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }

        /*========================================================================================================================
           11 ACTION CHE RITORNA UNA PARTIAL VIEW PER LE IMMAGINI IN GALLERIA .
        ========================================================================================================================*/
        [HttpGet]
        public ActionResult GetProductImagesPartial(int ProductId)
        {
            int idUser = 0;
            try
            {
                if (Session["UserId"] != null) idUser = (int)Session["UserId"];

                ProductCategoryViewModel model = new ProductCategoryViewModel
                {
                    GetProductFileByIdList = _ProductsService.sp_GetProductFileById(ProductId, false, "I"),
                };

                return PartialView("_Images", model);
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return new HttpStatusCodeResult(500);
            }
        }

        /*========================================================================================================================
          12  ACTION CHE RITORNA UNA PARTIAL VIEW DEI FILE
       ========================================================================================================================*/
        [HttpGet]
        public ActionResult GetProductFilesPartial(int ProductId)
        {
            int idUser = 0;
            try
            {
                if (Session["UserId"] != null) idUser = (int)Session["UserId"];

                ProductCategoryViewModel model = new ProductCategoryViewModel
                {
                    GetManualsList = _ProductsService.sp_GetProductFileById(ProductId, false, "M"),
                };

                return PartialView("_Files", model);
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return new HttpStatusCodeResult(500);
            }
        }

        /*=======================================================================================================================
         ************************************************************************************************************************
            13 ************************ ACTION CHE SI OCCUPA DELL'UPLOAD DELLE IMMAGINI **************************************
         ************************************************************************************************************************
         ======================================================================================================================*/

        [HttpPost]
        public JsonResult UploadProductImage(sp_ManageProductFile_Param param, HttpPostedFileBase uploadFiles)
        {
            int idUser = 0;
            try
            {
                idUser = (int)Session["UserId"];

                if (uploadFiles == null || uploadFiles.ContentLength == 0)
                {
                    return Json(new { success = false, message = "Nessun file selezionato." });
                }

                // 1. Validazione di sicurezza per sole immagini
                string extension = System.IO.Path.GetExtension(uploadFiles.FileName)?.ToLower();
                string[] allowedImgExtensions = { ".jpg", ".jpeg", ".png", ".webp" };

               

                if (!allowedImgExtensions.Contains(extension) || !uploadFiles.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
                {
                    return Json(new { success = false, message = "Selezionare un formato immagine valido (JPG, PNG, WEBP)." });
                }

                // 2. LETTURA CROP E REISZE DELL'IMMAGINE
                byte[] fileBytes;
                using (MemoryStream ms = new MemoryStream())
                {
                    uploadFiles.InputStream.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }

                // 3. CHIAMATA DEL VALIDATION SERVICE
                var valResult = _validationService.ValidateImage(fileBytes,uploadFiles.FileName);

             
                // SE LA VALIDAZIONE FALLISCE INTERROMPE L'ESECUZIONE E INVIA L'ERRORE AL CLIENT
                if (!valResult.IsValid)
                {
                    return Json(new { success = false, message = valResult.ErrorMessage });
                }


                // ISTRUZIONE CHE APPLICA IL CROP DELL'IMMAGINE E IL RESIZE
                byte[] editedImage = ImageHelper.CropAndResize(fileBytes);

                // APPLICAZIONE DEL WATERMARK ALL'IMMAGINE
                string logoPath = Server.MapPath("~/Content/Image/logo-watermark.png");
                editedImage = ImageHelper.ApplyWatermark(editedImage, logoPath);

                // 3. Preparazione parametri per la SP
                param.Operation = "I";
                param.FileContent = editedImage;
                param.FileSizeBytes = editedImage.Length;
                param.FileType = "Image";
                param.FileName = System.IO.Path.GetFileName(uploadFiles.FileName);
                param.FileExtension = extension;
                param.MimeType = uploadFiles.ContentType;

                // RICHIAMO DELLA MANAGE  PER INVIARE I DATI AL DATABASE
                _ProductsService.ManageProductFile(param);


                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }

        /*======================================================================================================================
          14  ACTION CHE SI OCCUPA DELL'UPLOAD DEI FILE
         ======================================================================================================================*/

        [HttpPost]
        public JsonResult UploadProductDocument(sp_ManageProductFile_Param param, HttpPostedFileBase uploadFiles)
        {
            int idUser = 0;
            try
            {
                idUser = (int)Session["UserId"];

                if (uploadFiles == null || uploadFiles.ContentLength == 0)
                {
                    return Json(new { success = false, message = "Nessun documento selezionato." });
                }

                // 1. Validazione di sicurezza per i documenti (es. solo PDF)
                string extension = System.IO.Path.GetExtension(uploadFiles.FileName)?.ToLower();
                string[] allowedDocExtensions = { ".pdf" };

                if (!allowedDocExtensions.Contains(extension))
                {
                    return Json(new { success = false, message = "Formato non supportato. È possibile caricare solo documenti PDF." });
                }

                // 2. Lettura del file originale senza alterazioni
                byte[] fileBytes;
                using (MemoryStream ms = new MemoryStream())
                {
                    uploadFiles.InputStream.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }

                // 3. Preparazione parametri per la SP
                param.Operation = "I";
                param.FileContent = fileBytes;
                param.FileSizeBytes = uploadFiles.ContentLength;
                param.FileType = "Manual";
                param.FileName = System.IO.Path.GetFileName(uploadFiles.FileName);
                param.FileExtension = extension;
                param.MimeType = uploadFiles.ContentType;

                _ProductsService.ManageProductFile(param);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }

        /*======================================================================================================================
          15  ACTION CHE SI OCCUPA DELLA GALLERIA DELLE IMMAGINI NELLA FINESTRA DI DETTAGLIO
         ======================================================================================================================*/
        [HttpGet]
        public ActionResult ImageDetails(int id, int productId)
        {
            int idUser = 0;

            try
            {
                idUser = (int)Session["UserId"];

                return Json(new { success = true, data = _ProductsService.sp_GetImageFileById(id, productId) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, ex.Message);

                // 3. Essendo una chiamata AJAX, restituisco l'errore in JSON, non una View!
                return Json(new { success = false, message = "Errore durante il caricamento del dettaglio." }, JsonRequestBehavior.AllowGet);
            }
            
        }

        /*======================================================================================================================
          16  ACTION CHE SETTA L'IMMAGINE PRINCIPALE
         ======================================================================================================================*/

        [HttpPost]
        public JsonResult SetMainProductFile(int id, int productId, string fileType = "Image")
        {
            int idUser = Session["UserId"] != null ? Convert.ToInt32(Session["UserId"]) : 0;

            try
            {
                if (id <= 0 || productId <= 0)
                {
                    return Json(new { success = false, message = "Dati dell'immagine non validi." });
                }

                // Chiamata alla tua Stored Procedure con Operation = 'U' e IsMain = true
                sp_ManageProductFile_Param param = new sp_ManageProductFile_Param
                {
                    Operation = "U",      // Update
                    Id = id,
                    ProductId = productId,
                    FileType = fileType,
                    IsMain = true         //  La SP resetterà le altre a 0 e questa a 1
                };

                _ProductsService.ManageProductFile(param);

                return Json(new {success = true, message = "Immagine principale aggiornata con successo!"});
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, "SetMainProductFile: " + ex.Message);
                return Json(new
                {
                    success = false,
                    message = "Errore durante l'impostazione dell'immagine principale."
                });
            }
        }

        /*======================================================================================================================
         17  ACTION CHE SETTA L'IMMAGINE PRINCIPALE
        ======================================================================================================================*/
        [HttpPost]
        public JsonResult SetMainProductFilePdf(int id, int productId, string fileType = "Image")
        {
            int idUser = Session["UserId"] != null ? Convert.ToInt32(Session["UserId"]) : 0;

            try
            {
                if (id <= 0 || productId <= 0)
                {
                    return Json(new { success = false, message = "Dati deL File non validi." });
                }

                // Chiamata alla tua Stored Procedure con Operation = 'U' e IsMain = true
                sp_ManageProductFile_Param param = new sp_ManageProductFile_Param
                {
                    Operation = "U",      // Update
                    Id = id,
                    ProductId = productId,
                    FileType = fileType,
                    IsMain = true         //  La SP resetterà le altre a 0 e questa a 1
                };

                _ProductsService.ManageProductFile(param);

                return Json(new { success = true, message = "Immagine principale aggiornata con successo!" });
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, "SetMainProductFile: " + ex.Message);
                return Json(new
                {
                    success = false,
                    message = "Errore durante l'impostazione dell'immagine principale."
                });
            }
        }


        /*======================================================================================================================
          18  ACTION CHE MODIFICA L'ORDINE
         ======================================================================================================================*/

        [HttpPost]
    
        public ActionResult UpdateImageOrder(int productId, List<ImageOrderDto> items)
        {
            int idUser = Session["UserId"] != null ? Convert.ToInt32(Session["UserId"]) : 0;

            try
            {
                if (items != null && items.Count > 0)
                {
                    foreach (var item in items)
                    {
                        sp_ManageProductFile_Param param = new sp_ManageProductFile_Param
                        {
                            Operation = "U",
                            Id = item.Id,
                            ProductId = productId,
                            DisplayOrder = item.DisplayOrder,
                            FileType = "Image"
                        };

                        _ProductsService.ManageProductFile(param);
                    }
                }

                // Ricarichiamo la lista aggiornata per restituire il parziale
                ProductCategoryViewModel model = new ProductCategoryViewModel
                {
                    GetProductFileByIdList = _ProductsService.sp_GetProductFileById(productId, false, "I")
                };

                return PartialView("_Images", model);
            }
            catch (Exception ex)
            {
                _logService.WriteLogProcessError(idUser, "UpdateImageOrder: " + ex.Message);
                Response.StatusCode = 500;
                return Content("Errore durante il riordinamento: " + ex.Message);
            }
        }

    }
}
