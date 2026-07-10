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
    public class ProductCategoryService : IProductCategoryService
    {
        DAL_ProductCategory ProductCategory = new DAL_ProductCategory();


    public  List<sp_SearchProductCategory_Result> sp_SearchProductCategory(sp_SearchProductCategory_Param spParam)
    {
           return ProductCategory.sp_SearchProductCategory(spParam);
    }

    public void ManageProductCategory(sp_ManageProductCategory_Param param)
    {
            ProductCategory.ManageProductCategory(param);
    }

    

    }
}
