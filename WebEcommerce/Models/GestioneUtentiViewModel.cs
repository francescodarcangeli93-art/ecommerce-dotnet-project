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
    }

    public class WarehousesViewModel
    {
        public List<sp_SearchWarehouses_Result> WarehousesList { get; set; }

        public sp_GetSupplierById_Result TheWarehouses { get; set; }

     
    }
}