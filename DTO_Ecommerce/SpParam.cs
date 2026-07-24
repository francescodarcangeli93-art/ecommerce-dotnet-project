using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace DTO_Ecommerce
{
    /*   public class SpOutputParam
       {
           public string RC { get; set; }
           public string RMessage { get; set; }
       }
    */

    public class ImageOrderDto
    {
        public int Id { get; set; }
        public int DisplayOrder { get; set; }
    }
    public class sp_ManageLog_Param
    {
        public int? IdUser {  get; set; }
        public Enums.typeLog IdLogType { get; set; }
        public Enums.severityLog IdLogSeverity { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class sp_CheckProductReorderInWarehouse_Param
    {
        public int Id { get; set; }
        public int WarehouseId { get; set; }
    }

    public class sp_ClearCartByCustomerId_Param_Param
    {
        public int Id { get; set; }
    }

    public class sp_CreateOrderFromCart_Param
    {
        public int Id { get; set; }
        public byte OrderStatusId { get; set; }
    }

    public class sp_CreateShipmentFromOrder_Param
    {
        public int Id { get; set; }
        public int UserAddressId { get; set; }
        public byte ShipmentStatusId { get; set; }
        public byte MovementReasonTransferId { get; set; }
        public byte MovementReasonSaleId { get; set; }
    }
    public class sp_GetAllCategoryProducts_Param
    {

        public int? IdProductCategory { get; set; }
        public bool AllData { get; set; }
        public string FileFilter { get; set; }


    }

    public class sp_GetCardResponse_Param
    {
        public string CardHolderName { get; set; }
        public string CardHolderSurname { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string Cvv { get; set; }
    }
    public class sp_GetCustomerAddresses_Param
    {
        public int? CustomerId { get; set; }

    }

    public class sp_GetImageFileById_Param
    {
        public int? Id { get; set; }
        public int? ProductId { get; set; }

    }


    public class sp_GetCustomerById_Param
    {
        public int? Id { get; set; }
    }
   

    public class sp_GetCustomerCart_Param
    {
        public int? CustomerId { get; set; }
    }

    public class sp_GetCustomerOrders_Param
    {
        public int? CustomerId { get; set; }
    }

    public class sp_GetMainProductImage_Param
    {
        public int? ProductId { get; set; }
    }

    public class sp_GetMainProductImageOrProductManual_Param
    {
        public int? ProductId { get; set; }

        public string FileType { get; set; }
    }

    public class sp_GetMovementReasonById_Param
    {
        public int? Id { get; set; }
    }


    public class sp_GetOrderById_Param
    {
        public int? OrderId { get; set; }
        public int? OrderItemId { get; set; }
        public string Mode { get; set; }
    }

    public class sp_GetPaymentStatusById_Param
    {
        public int? Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

    }
    public class sp_GetPaymentTypeById_Param
    {
        public byte Id { get; set; }
        public string FilterType { get; set; }

    }
    public class sp_GetPriceListById_Param
    {
        public int? PriceListId { get; set; }
        public int? PriceListItemId { get; set; }
        public int? ProductId { get; set; }
        public string @Mode { get; set; }

    }

    public class sp_GetPriceListItemById_Param
    {
        public int? Id { get; set; }
       

    }

    public class sp_GetProductById_Param
    {
        public int? IdProduct { get; set; }

    }
    public class sp_GetProductCategoryById_Param
    {
        public int? Id { get; set; }

    }
    public class sp_GetProductFileById_Param
    {
        public int? ProductId { get; set; }
        public bool IncludeInactive { get; set; }

        public string FileType { get; set; }

    }

    public class sp_GetProductFileContent_Param
    {
        public int Id { get; set; }
        
    }
    public class sp_GetShipmentStatusById_Param
    {
        public short Id { get; set; }

    }
    public class sp_GetShippingCompanyById_Param
    {
        public int? Id { get; set; }

    }
    public class sp_GetSupplierById_Param
    {
        public int? IdSupplier { get; set; }

    }
    public class sp_GetUserById_Param
    {
        public int? Id { get; set; }

    }
    public class sp_GetUserRoleById_Param
    {
        public int? Id { get; set; }

    }
    public class sp_GetWarehouseById_Param
    {
        public int? Id { get; set; }

    }

    public class sp_GetWarehouseMovementsById_Param
    {
        public int WarehouseId { get; set; }
    }

    public class sp_GetWarehouseStockProductById_Param
    {
        public int? Id { get; set; }
    }

    public class sp_LoginUser_Param
    {
        public string Username { get; set; }
        public string Password { get; set; }

    }

    public class sp_ManageCartItem_Param
    {
        public string Operation { get; set; }
        public int? Id { get; set; }
        public int? CartId { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
        public bool IsAvailable { get; set; }

    }
    public class sp_ManageCustomer_Param
    {
        public string Operation { get; set; }
        public int? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }

        public string Country { get; set; }
        public string Phone { get; set; }

        public int? CustomerIdInserted { get; set; }
    }

    public class sp_ManageCustomerAddress_Param
    {
        public string Operation { get; set; }
        public int? Id { get; set; }
        public int? CustomerId { get; set; }
        public string Address { get; set; }
        public string Postalcode { get; set; }
        public string City { get; set; }
        public bool? IsDefault { get; set; }
        public bool? @IsBilling { get; set; }
    }

    public class sp_ManageMovementReason_Param
    {
        public string Operation { get; set; }
        public short? Id { get; set; }
        public short? @ParentId { get; set; }
        public string @Code { get; set; }
        public string @Description { get; set; }
        public string @MovementType { get; set; }
    }

    public class sp_ManageOrder_Param
    {
        public string Operation { get; set; }
        public int? Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderNumber { get; set; }
        public int? CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
    }
    public class sp_ManageOrderItem_Param
    {
        public string Operation { get; set; }
        public int? Id { get; set; }
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public int? Quantity { get; set; }

    }
    public class sp_ManagePaymentStatus_Param
    {
        public string Operation { get; set; }
        public short Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

    }

    public class sp_ManagePaymentType_Param
    {
        public string Operation { get; set; }

        public short Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }

    public class sp_ManagePriceList_Param
    {
        public string Operation { get; set; }
        public int? Id { get; set; }
        public string PriceListName { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string CurrencyCode { get; set; }
        public bool? IsDefault { get; set; }
        public bool? IsActive { get; set; }

    }

    public class sp_ManagePriceListItem_Param
    {
        public string Operation { get; set; }
        public int? Id { get; set; }
        public int? PriceListId { get; set; }
        public int? ProductId { get; set; }
        public decimal? Price { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
    public class sp_ManageProduct_Param
    {
        public string Operation { get; set; }
        public int? Id { get; set; }
        public string ProductName { get; set; }
        public int? SupplierId { get; set; }
        public int? CategoryId { get; set; }
        public decimal? BasedUnitPrice { get; set; }
        public string Package { get; set; }
        public bool? IsDiscontinued { get; set; }
        public bool? VisibleInHP { get; set; }
    }
    public class sp_ManageProductCategory_Param
    {
        public string Operation { get; set; }
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }

    }

    public class sp_ManageProductFile_Param
    {
        public string Operation { get; set; }     
        public int? Id { get; set; }               
        public int? ProductId { get; set; }        
        public string FileType { get; set; }     
        public string FileName { get; set; }     
        public string FileExtension { get; set; } 
        public string MimeType { get; set; } 
        public string Title { get; set; }
        public byte[] FileContent { get; set; }   
        public long? FileSizeBytes { get; set; }   
        public string Description { get; set; }  
        public bool? IsMain { get; set; }        
        public int? DisplayOrder { get; set; }     
        public bool? IsActive { get; set; }       
    }
    public class sp_ManageShipmentStatus_Param
    {
        public string Operation { get; set; }
        public short Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        

    }
    public class sp_ManageSupplier_Param
    {
        public string Operation { get; set; }
        public int? Id { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
    }

    public class sp_ManageUser_Param
    {
        public string Operation { get; set; }
        public int? Id { get; set; }
        public int? RoleId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }
        public int? CustomerId { get; set; }
    }
    public class sp_ManageUserRole_Param
    {
        public string Operation { get; set; }
        public int? Id { get; set; }
        public string @RoleName { get; set; }
        public string @Description { get; set; }
        public bool @IsActive { get; set; }
      
    }

    public class sp_ManageWarehouse_Param
    {
        public string Operation { get; set; }
        public int? Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public bool? IsActive { get; set; }

    }

    public class sp_SearchCustomer_Param
    {
        public string @FirstName { get; set; }
        public string @LastName { get; set; }
        public string @City { get; set; }
        public string @Country { get; set; }
        public string @Phone { get; set; }

    }

    public class sp_SearchMovementReasons_Param
    {
        public short @ParentId { get; set; }
        public string @Code { get; set; }
        public string @Description { get; set; }
        public string @MovementType { get; set; }
    

    }

    public class sp_SearchOrder_Param
    {
        public DateTime OrderDate { get; set; }
        public string OrderNumber { get; set; }
        public int? CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public int? Quantity { get; set; }
    }

    public class sp_SearchPaymentStatus_Param
    {
        public string Code { get; set; }
        public string Description { get; set; }
       
    }
    public class sp_SearchPaymentType_Param
    {
        public string Code { get; set; }
        public string Description { get; set; }

        public bool IsActive { get; set; }

    }

    public class sp_SearchPriceList_Param
    {
        public string PriceListName { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string CurrencyCode { get; set; }
        public bool? IsDefault { get; set; }
        public bool? IsActive { get; set; }

    }

  

    public class sp_SearchProduct_Param
    {
        public string ProductName { get; set; }
        public int? SupplierId { get; set; }
        public decimal? BasedUnitPriceDa { get; set; }
        public decimal? BasedUnitPriceA { get; set; }
        public string Package { get; set; }
        public bool? IsDiscontinued { get; set; }

        public int? CategoryId { get; set; }
    }

    public class sp_SearchProductCategory_Param
    {
    
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public int? DisplayOrder { get; set; }
        public string Description { get; set; }
    }
    public class sp_SearchProductFile_Param
    {
        public int? ProductId { get; set; }
        public int? Id { get; set; }
       
    }
    public class sp_SearchShipmentStatus_Param
    {
        public string Code { get; set; }
        public string Description { get; set; }

    }
    public class sp_SearchShippingCompany_Param
    {
        public string CompanyName { get; set; }
        public bool IsActive { get; set; }
        public bool IsFreeShipping { get; set; }

    }
    public class sp_SearchSupplier_Param
    {
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
    }
    public class sp_SearchUser_Param
    {
        public int? Id { get; set; }
        public int? RoleId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool? IsActive { get; set; }
        public int? CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    
    }
    public class sp_SearchUserRole_Param
    {
        public int? Id { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
       
    }

    public class sp_SearchWarehouses_Param
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Type { get; set; }
        public bool? IsActive { get; set; }

    }

    public class sp_SearchWarehousesMovements_Param
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? WarehouseId { get; set; }
        public int? ProductId { get; set; }
        public string MovementType { get; set; }
        public int? MovementReasonId { get; set; }

        public string CompanyName { get; set; }

    }


    public class sp_WarehouseMoveProducts_Param
    {
        public short MovementReasonId { get; set; }
        public int? ProductId { get; set; }
        public int? QtyToTransfer { get; set; }
        public int? WarehouseId { get; set; }
        public int? DestinationWarehouseId { get; set; }

    }

   


    // --------------   MANAGE  --------------------------



}
