using BLL_Ecommerce.Interfaces;
using BLL_Ecommerce.Services;
using DTO_Ecommerce;
using System.Web.Mvc;
using WebEcommerce.Models;

namespace WebEcommerce.Controllers
{
    public class GestSupplierController : Controller
    {

        private readonly ISupplierService _supplierService;

        public GestSupplierController ()
        {
            _supplierService = new SupplierService ();
        }



        // GET: Supplier
        public ActionResult Index()
        {
            CheckSession();

            sp_SearchSupplier_Param param = new sp_SearchSupplier_Param();

            SupplierViewModel Model = new SupplierViewModel
            {
                SupplierList = _supplierService.SearchSupplier(param)
            };

           

            return View(Model);
        }

        // GET: Supplier/Details
        public ActionResult Details(int id)
        {
            CheckSession();
            return View();
        }

        // GET: Supplier/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Supplier/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Supplier/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Supplier/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Supplier/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Supplier/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private ActionResult CheckSession()
        {
            if (Session["UserId"] == null)
            {
               return RedirectToAction("Index", "Login");
            }
            return null;
        }

    }
}
