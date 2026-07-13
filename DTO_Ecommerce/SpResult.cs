using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;

namespace DTO_Ecommerce
{
    /*  public class Get_UsersList_Result
      {
          public int IdUser { get; set; }
          public bool Attivo { get; set; }
          public string Username { get; set; }
          public string Password { get; set; }
          public string Nome { get; set; }
          public string Cognome { get; set; }
          public string Email { get; set; }
          public string Telefono { get; set; }
          public DateTime DataInizioValidita { get; set; }
          public DateTime? DataFineValidita { get; set; }
          public string CodiceRuolo { get; set; }
          public string DescrizioneRuolo { get; set; }
       }
     */


    public class sp_CheckProductReorderInWarehouse_Result
    {
        public int? Quantity { get; set; }
    }

    public class sp_GetAllCategoryProducts_Result
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int? DisplayOrder { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? IdProduct { get; set; }

    }

    public class sp_GetAllCategoryProducts_Detail_Result : sp_GetAllCategoryProducts_Result
    {
        //Proprieta Prodotto
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal BasedUnitPrice { get; set; }
        public string Package { get; set; }
        public bool IsDiscontinued { get; set; }

        // Proprieta ProductFile
        public int? FileId { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
        public string FileTitle { get; set; }
        public bool IsMain { get; set; }
        public int FileDisplayOrder { get; set; }
    }

    public class sp_GetCardResponse_Result
    {
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
       

    }

    public class sp_GetCustomerAddresses_Result
    {
        public int? Id { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public bool IsDefault { get; set; }
        public bool IsBilling { get; set; }
        
       
    }

    public class sp_GetCustomerById_Result
    {
        public int? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
      
    }
    public class sp_GetCustomerCart_Result
    {
        public int? Id { get; set; }
        public int? CustomerId { get; set; }
        public int? NumProducts { get; set; }
        public int? ProductId { get; set; }
        public decimal BasedUnitPrice { get; set; }
        public string Package { get; set; }
        public string ProductName { get; set; }

    }



    public class sp_GetCustomerOrders_Result
    {
        public int? Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }

    }

    public class sp_GetListCountry_Result
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string CountryCode { get; set; }
        
    }

    public class sp_GetListUserRole_Result
    {
        public int? Id { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
       
    }

    public class sp_GetMainProductImageOrProductManual_Result
    {
        public int? Id { get; set; }
        public int? ProductId { get; set; }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
        public long? FileSizeKb { get; set; }
        public string Title { get; set; }
  
    }

    public class sp_GetMovementReasonById_Result
    {
        public int? Id { get; set; }
        public int? WarehouseId { get; set; }
        public int? MovementReasonId { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
        public char MovementType { get; set; }

    }

    public class sp_GetOrderById_Result
    {
        public sp_GetOrderByIdHeader_Result OrderHeader = new sp_GetOrderByIdHeader_Result();

        public List<sp_GetOrderByIdDetail_Result> OrderDetails = new List<sp_GetOrderByIdDetail_Result>();
    }

    public class sp_GetOrderByIdHeader_Result
    {
        public int? Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderNumber { get; set; }
        public int? CustomerId { get; set; }
        public decimal? TotalAmount { get; set; }

    }

    public class sp_GetOrderByIdDetail_Result
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal LineTotal { get; set; }
    }



    public class sp_GetPaymentStatusById_Result
    {
        public int? Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
       
    }

    public class sp_GetPaymentTypeById_Result
    {
        public int? Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

    }

    public class sp_GetPriceListByIdHeader_Result
    {
        public int Id { get; set; } // se non c'è riga, Header resta null nel container: qui non serve int?
        public string PriceListName { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }   // reso nullable: stessa colonna di HeaderEndDate sotto
        public string CurrencyCode { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }

        // queste proprietà servono per il json
        public string StartDateString => StartDate.ToString("yyyy-MM-dd");
        public string EndDateString => EndDate.HasValue ? EndDate.Value.ToString("yyyy-MM-dd") : null;
    }

    public class sp_GetPriceListByIdDetail_Result
    {
        //public int Id { get; set; } // Id del PriceListItem (PT.Id)
        //public string ProductName { get; set; } = string.Empty;
        //public int PriceListId { get; set; }
        //public int ProductId { get; set; }
        //public decimal Price { get; set; }
        //public int MinQuantity { get; set; }
        //public DateTime StartDate { get; set; }
        //public DateTime? EndDate { get; set; }

        //// --- Campi della Testata ripetuti nella JOIN (P) ---
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int MinQuantity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class sp_GetPriceListById_Result
    {
        public sp_GetPriceListByIdHeader_Result Header { get; set; }
        public List<sp_GetPriceListByIdDetail_Result> Details { get; set; }
    }

    public class sp_GetPriceListItemById_Result
    {
        public int Id { get; set; }
        public int PriceListId { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int MinQuantity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string StartDateString => StartDate.ToString("yyyy-MM-dd");
        public string EndDateString => EndDate.HasValue ? EndDate.Value.ToString("yyyy-MM-dd") : null;
    }

    public class sp_GetProductById_Result
    {
        public int? Id { get; set; }
        public string ProductName { get; set; }
        public int SupplierId { get; set; }
        public decimal BasedUnitPrice { get; set; }
        public string Package { get; set; }
        public bool IsDiscontinued { get; set; }
        public bool VisibleinHP { get; set; }

    }

    public class GetProductCategoryById_Result
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }

    }

    public class sp_GetProductFileById_Result
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } 
        public string FileType { get; set; }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
        public long FileSizeKb { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsMain { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class sp_GetShipmentStatusById_Result
    {
        public int Code { get; set; }
        public int Description { get; set; }
       
    }

    public class sp_GetShippingCompanyById_Result
    {
        public string CompanyName { get; set; }
        public bool IsActive { get; set; }
        public bool IsFreeShipping { get; set; }
    }

    public class sp_GetSupplierById_Result
    {
        public int? Id { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
    }

    public class sp_GetUserById_Result
    {
        public int? Id { get; set; }
        public int? RoleId { get; set; }
        public string RoleName { get; set; } 
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public int? CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public string Phone { get; set; }

        public string IconUserRole
        {
            get
            {
                switch (this.RoleName.ToUpper())
                {
                    case "ADMIN":
                    {
                        return "bi bi-shield-lock-fill";
                    }
                    case "CUSTOMER":
                    {
                        return "bi bi-person-circle";
                    }
                    case "ADMIN-CUSTOMER":
                    {
                        return "bi bi-person-lock";
                    }
                    default:
                    {
                        return "";
                    }
                }
            }
        }

        public bool ShowAddressIcon
        {
            get
            {
                switch (this.RoleName.ToUpper())
                {
                    case "ADMIN":
                    {
                        return false;
                    }
                    default:
                    {
                        return true;
                    }
                }
            }
        }
    }

    public class sp_GetUserRoleById_Result
    {
        public int? Id { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }

    public class sp_GetWarehouseById_Result
    {
        public int? Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public bool IsActive { get; set; }
    }

    public class sp_GetWarehouseMovementsById_Result
    {
        public int WarehouseId { get; set; }
        public DateTime Date { get; set; }
        public DateTime LastUpdate { get; set; }
        public string DescrizioneVisualizzata { get; set; } 
        public string Code { get; set; }                
        public int ProductId { get; set; }                     
        public string ProductName { get; set; }
        public int QuantitaStock { get; set; }             
        public int QuantitaMovimento { get; set; }  
        public string MovementType { get; set; }         
        public int? SupplierId { get; set; }
        
        public string ColorClass
        {
            get
            {
                // Usiamo ToUpper() per evitare bug se sul DB è scritto "carico" minuscolo
                switch (this.Code.ToUpper().Trim())
                {
                    case "CARICO":
                        return "lText-green";

                    case "RETTIFICA+":
                        return "lText-green";
                    case "FURTO":
                        return "lText-yellow";
                    case "INCENDIO":
                        return "lText-red";
                    default:
                        return "lText-neutral"; // Colore neutro di sicurezza
                }
            }
        }
    }

    // SEARCH 
    public class sp_SearchCustomer_Result
    {
        public int? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }


    }

    public class sp_SearchOrder_Result
    {
        public DateTime OrderDate { get; set; }
        public string OrderNumber { get; set; }
        public int? CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public int? Id { get; set; } 
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public int? Quantity { get; set; }
    }

    public class sp_SearchPaymentStatus_Result
    {
        public int? Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class sp_SearchPaymentType_Result
    {
        public int? Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }

    public class sp_SearchPriceList_Result
    {
        public int Id { get; set; }
        public string PriceListName { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string CurrencyCode { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }

        public string DefaultIconClass => IsDefault ? "bi bi-star-fill text-warning" : "";

        //  Property calcolata per la badge di stato
        public string ActiveBadgeClass => this.IsActive ? "bg-success" : "bg-secondary";
        public string ActiveStatusText => this.IsActive ? "Attivo" : "Inattivo";
    }

    public class sp_SearchProduct_Result
    {
        public int? Id { get; set; }
        public string ProductName { get; set; }
        public int? SupplierId { get; set; }
        public decimal BasedUnitPrice { get; set; }
        public string Package { get; set; }
        public bool IsDiscontinued { get; set; }


    }

    public class sp_GetProductCategoryById_Result
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class sp_SearchProductCategory_Result
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class sp_SearchProductFile_Result
    {
        public int? Id { get; set; }
        public int? ProductId { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
        public int? FileSizeKb { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsMain { get; set; }
        public int? DisplayOrder { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Stato { get; set; }
    }

    public class sp_SearchShipmentStatus_Result
    {
        public int? Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class sp_SearchShippingCompany_Result
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public bool IsActive { get; set; }
        public bool IsFreeShipping { get; set; }
    }

    public class sp_SearchSupplier_Result
    {
        public int? Id { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; } 
        public string Phone { get; set; }
        public string Fax { get; set; }
    }

    public class sp_SearchUser_Result
    {
        public int? Id { get; set; }
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public int? CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string IconUserRole
        {
            get
            {
                switch (this.RoleName.ToUpper())
                {
                    case "ADMIN":
                        {
                            return "bi bi-shield-lock-fill";
                        }
                    case "CUSTOMER":
                        {
                            return "bi bi-person-circle";
                        }
                    case "ADMIN-CUSTOMER":
                        {
                            return "bi bi-person-lock";
                        }
                    default:
                        {
                            return "";
                        }
                }
            }
        }

        public int? Indirizzi { get; set; }

        public string emptyName
        {
            get
            {
                // Controlla se FirstName è null o vuoto; se sì mette "-- --", altrimenti mette il valore reale
                return string.IsNullOrWhiteSpace(this.FirstName) ? "-- --" : this.FirstName;
            }
        }

        public string emptyLastName
        {
            get
            {
                // Controlla se LastName è null o vuoto; se sì mette "-- --", altrimenti mette il valore reale
                return string.IsNullOrWhiteSpace(this.LastName) ? "-- --" : this.LastName;
            }
        }
    }

    public class sp_SearchUserRole_Result : sp_GetUserRoleById_Result
    {

    }

    public class sp_SearchWarehouses_Result : sp_GetWarehouseById_Result
    {

    }

    public class sp_LoginUser_Result
    {
        public int UserId { get; set; }
        public int? CustomerId { get; set; }
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public string RoleDescription { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }


    }
}
