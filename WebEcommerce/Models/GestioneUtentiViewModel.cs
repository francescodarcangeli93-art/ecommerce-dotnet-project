using DTO_Ecommerce;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebEcommerce.Models
{
    public class GestioneUtentiViewModel                                  
    {

        public GestioneUtentiViewModel()
        {
            Filtri = new sp_SearchUser_Param();
            ListRuoli = new List<sp_GetListUserRole_Result>();
            ListaUtenti = new List<sp_SearchUser_Result>();
        }

        public List <sp_GetListUserRole_Result> ListRuoli { get; set; }
        public sp_SearchUser_Param Filtri { get; set; }
        public List<sp_GetCustomerAddresses_Result> CustomerAddresses { get; set; }
        public List<sp_SearchUser_Result> ListaUtenti { get; set; }

        public sp_GetUserById_Result UserDetail;

        

        // singoli valori 
        public int? Id { get; set; }

        public int? RoleId { get; set; }

        public string RoleName { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public DateTime? CreatedDate { get; set; }

        public bool IsActive { get; set; }

      

        public int? CustomerId { get; set; }

        public string Icona
        {

            get
            {
                if (CustomerId == null)
                {
                    return "Utente";
                }
                else
                {
                    return "Customer";
                }
            }

        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string Phone { get; set; }

    }

    public class SupplierViewModel
    {
        public List<sp_SearchSupplier_Result> SupplierList {  get; set; }

        public sp_GetSupplierById_Result TheSupplier { get; set; }

        public List<sp_GetListCountry_Result> Country { get; set; }

        public List<sp_GetListSupplier_Result> TheSupplierList { get; set; }
    }

    public class WarehousesViewModel
    {
        public List<sp_SearchWarehouses_Result> WarehousesList { get; set; }

        public sp_GetWarehouseById_Result TheWarehouse { get; set; }

        public List<sp_GetListProduct_Result> TheProductsList { get; set; }
       public List<sp_GetWarehouseMovementsById_Result> TheWarehouseMovementsList { get; set; }

       public List<sp_GetWarehouseStockProductById_Result> TheWarehouseStock {  get; set; }

        /*===============================================================================================================
         *** ATTRIBUTI PER I MOVIMENTI DEI MAGAZZINI ***
        ================================================================================================================ */

        public List<sp_SearchWarehousesMovements_Result> TheWarehouseMovements { get; set; }

        public int TotCount { get; set; }

        public int TotalPage { get; set; }

        public int CurrentPage { get; set; }


        public List<sp_GetMovementReasonList_Result> TheMovementReasonList { get; set; }

        public List<sp_GetAllWareHouseList_Result> sp_GetAllWareHouseList {  get; set; }

        public sp_GetWarehousesMovementsById_Result TheGetWarehouseMovementDetail {  get; set; }
    }

    public class PriceListViewModel
    {
        public List<sp_SearchPriceList_Result> ThePriceList { get; set; }

        public sp_GetPriceListByIdHeader_Result Header { get; set; }
        public List<sp_GetPriceListByIdDetail_Result> Details { get; set; }

    }

    public class ProductCategoryViewModel
    {
        public List<sp_SearchProductCategory_Result> TheProductCategory { get; set; }
        public sp_GetProductCategoryById_Result ThetCategory { get; set; }

        public List<sp_GetListProduct_Result> ListProduct { get; set; }


        /*========================================================================================
                ******* CONTEINER FOR PRODUCTS *******
         =========================================================================================*/
        public List<sp_SearchProduct_Result> TheProducts { get; set; }

        public List<sp_GetListProductCategoryProduct_Result> ListProductCategories { get; set; }

        public sp_GetProductById_Result TheProduct {  get; set; }
        public List<sp_GetListSupplier_Result> TheSupList {  get; set; }
        public List<sp_GetListSupplierProduct_Result> TheSupplierList { get; set; }

        public List<sp_GetProductFileById_Result> GetProductFileByIdList { get; set; }
        public List<sp_GetProductFileById_Result> GetManualsList { get; set; }

        public sp_GetProductFileById_Result MainImage => GetProductFileByIdList.FirstOrDefault(x => x.IsMain);

        public List<sp_GetProductFileById_Result> SecondaryImages => GetProductFileByIdList.Where(x => !x.IsMain).ToList();

        // dettaglio immagini
        public sp_GetImageFileById_Result TheImageDetail {  get; set; }



    }
}