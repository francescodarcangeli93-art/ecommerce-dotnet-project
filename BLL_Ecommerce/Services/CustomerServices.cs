using BLL_Ecommerce.Interfaces;
using DAL_Ecommerce;
using DTO_Ecommerce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Ecommerce.Services
{
    public class CustomerServices : ILCustomerService
    {

        DAL_Customer Customer = new DAL_Customer();

        public int sp_ManageCustomer(sp_ManageCustomer_Param spParam)
        {
            return Customer.sp_ManageCustomer(spParam);
        }
    }


}
