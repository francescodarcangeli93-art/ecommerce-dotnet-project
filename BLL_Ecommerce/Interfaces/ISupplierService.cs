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
         List<sp_SearchSupplier_Result> SearchSupplier(sp_SearchSupplier_Param param);
         sp_GetSupplierById_Result GetSupplierById(int id);
         void sp_ManageSupplier (sp_ManageSupplier_Param param);
    }
}
