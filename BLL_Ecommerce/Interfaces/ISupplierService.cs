using DTO_Ecommerce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Ecommerce.Interfaces
{
    public interface ISupplierService
    {
        Task <List<sp_SearchSupplier_Result>> SearchSupplierAsync(sp_SearchSupplier_Param param);
        Task <sp_GetSupplierById_Result> GetSupplierByIdAsync(int id);
        Task sp_ManageSupplierAsync (sp_ManageSupplier_Param param);
    }
}
