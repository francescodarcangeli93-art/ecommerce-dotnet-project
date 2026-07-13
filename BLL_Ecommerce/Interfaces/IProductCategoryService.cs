using DTO_Ecommerce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Ecommerce.Interfaces
{
    public interface IProductCategoryService
    {
         List<sp_SearchProductCategory_Result> sp_SearchProductCategory(sp_SearchProductCategory_Param spParam);

         void ManageProductCategory(sp_ManageProductCategory_Param param);

        sp_GetProductCategoryById_Result GetProductCategoryById(int? id);

    }
}