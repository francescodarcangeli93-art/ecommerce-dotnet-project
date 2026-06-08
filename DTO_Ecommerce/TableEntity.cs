using System;

namespace DTO_Ecommerce
{

   
    public class Cart
    {
       public int id {  get; set; }
       public int UserId { get; set; }
       public int NumProduct { get; set; }
    }

    public class CartItem
    {
        public int id { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public bool IsAvaiable { get; set; }
    }

    public class CreditCardReference
    {
        public int Id { get; set; }
        public string CardholderName { get; set; }
        public string CardholderSurname { get; set; }
        public string CardNumber { get; set; }
        public string Circuit { get; set; }
        public string ExpiryDate { get; set; }
        public string Cvv { get; set; }
        public string CardType { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public bool IsActive { get; set; }
    }
    public class Customer
    {
        public int? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
    }
    public class CustomerAddress
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public bool IsDefault { get; set; }
        public bool IsBilling { get; set; }
    }

    public class MovementReason
    {
        public short Id { get; set; }
        public short ParentId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string MovementType { get; set; }
    }

    public class Order
        {
            public int Id { get; set; }
            public DateTime OrderDate { get; set; }
            public string OrderNumber { get; set; }
            public int CustomerId { get; set; }
            public short StatusId { get; set; }
            public decimal TotalAmount { get; set; }
        }
        public class OrderItem
        {
            public int Id { get; set; }
            public int OrderId { get; set; }
            public int ProductId { get; set; }
            public decimal UnitPrice { get; set; }
            public int Quantity { get; set; }
        }

        public class OrderStatus
        {
            public short Id { get; set; }
            public string Code { get; set; }
            public string Description { get; set; }
        }
        
     

        public class Payment
        {
            public int Id { get; set; }
            public int OrderId { get; set; }
            public int TypeId { get; set; }
            public DateTime Date { get; set; }
            public short StatusId { get; set; }
        }
        public class PaymentStatus
        {
            public short Id { get; set; }
            public string Code { get; set; }
            public string Description { get; set; }
        }

        public class PaymentType
        {
            public short Id { get; set; }
            public string Code { get; set; }
            public string Description { get; set; }
            public bool IsActive { get; set; }
        }
        public class PriceList
        {
            public int Id { get; set; }
            public string PriceListName { get; set; }
            public string Description { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string CurrencyCode { get; set; }
            public bool IsDefault { get; set; }
            public bool IsActive { get; set; }
        }
        
        

    public class PriceListItem
    {
        public int Id { get; set; }
        public int PriceListId { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int MinQuantity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int SupplierId { get; set; }
        public decimal BasedUnitPrice { get; set; }
        public string Package { get; set; }
        public bool IsDiscontinued { get; set; }
    }
   

    public class ProductCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class ProductCategory_Product
    {
        public int Id { get; set; }
        public int IdProductCategory { get; set; }
        public int IdProduct { get; set; }
    }
  
    public class ProductFile
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
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
    public class Shipment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public short CustomerAddressId { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string TrackingNumber { get; set; }
        public int WarehouseId { get; set; }
        public short ShipmentStatusId { get; set; }
        public int ShipmentCompanyId { get; set; }
        public decimal ShipmentCost { get; set; }
    }
    public class ShipmentItem
    {
        public int Id { get; set; }
        public int ShipmentId { get; set; }
        public int OrderItemId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
    public class ShipmentStatus
    {
        public short Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
    public class ShippingCompany
    {
        public int Id { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public bool IsActive { get; set; }
        public bool IsFreeShipping { get; set; }
        public decimal ShippingCost { get; set; }
        public string NumDaysToDeliver { get; set; }
    }
    public class Supplier
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
    }
    public class User
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public int? CustomerId { get; set; }
    }
    public class UserRole
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
    public class Warehouse
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public bool IsActive { get; set; }
    }
    public class WarehouseMovement
    {
        public int Id { get; set; }
        public int WarehouseId { get; set; }
        public short MovementReasonId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
        public string MovementType { get; set; }
    }
    public class WarehouseStockProduct
    {
        public int Id { get; set; }
        public int WarehouseId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int QuantityMinReorder { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
