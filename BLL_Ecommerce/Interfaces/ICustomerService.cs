using DTO_Ecommerce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Ecommerce.Interfaces
{
    public interface ICustomerService
    {
         int sp_ManageCustomer(sp_ManageCustomer_Param spParam);

         List<sp_GetCustomerAddresses_Result> GetCustomerAddresses(int? customerId);
        void sp_ManageCustomerAddress(sp_ManageCustomerAddress_Param spParam);

    }
}
