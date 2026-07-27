using DTO_Ecommerce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Ecommerce.Interfaces
{
    public interface IRoleService
    {
        List<sp_GetListUserRole_Result> UserRoleList();

        List<sp_GetListProduct_Result> sp_GetListProduct();
        List<sp_GetListProductCategory_Result> ListProductCategory();

        List<sp_GetListSupplier_Result> GetListSupplier();
    }
}
