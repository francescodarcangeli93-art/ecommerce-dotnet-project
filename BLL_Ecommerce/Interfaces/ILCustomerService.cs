using DTO_Ecommerce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Ecommerce.Interfaces
{
    public interface ILCustomerService
    {
         int sp_ManageCustomer(sp_ManageCustomer_Param spParam);
       
        
    }
}
