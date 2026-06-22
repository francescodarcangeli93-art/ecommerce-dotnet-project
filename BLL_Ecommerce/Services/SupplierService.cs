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

     

        public async Task <List<sp_SearchSupplier_Result>> SearchSupplierAsync(sp_SearchSupplier_Param param)
        {
             return await Supplier.SearchSupplier(param);
        }

        public async Task<sp_GetSupplierById_Result> GetSupplierByIdAsync(int id)
        {
           return await Supplier.GetSupplierById(id);
        }

        public async Task sp_ManageSupplierAsync(sp_ManageSupplier_Param param)
        {
            await Supplier.sp_ManageSupplier(param);
        }
    }
}
