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
    public class SupplierService : ISupplierService
    {
        DAL_Supplier Supplier = new DAL_Supplier();

     

        public   List<sp_SearchSupplier_Result> SearchSupplier(sp_SearchSupplier_Param param)
        {
             return  Supplier.SearchSupplier(param);
        }

        public  sp_GetSupplierById_Result GetSupplierById(int id)
        {
           return  Supplier.GetSupplierById(id);
        }

        public void sp_ManageSupplier(sp_ManageSupplier_Param param)
        {
             Supplier.sp_ManageSupplier(param);
        }
    }
}
