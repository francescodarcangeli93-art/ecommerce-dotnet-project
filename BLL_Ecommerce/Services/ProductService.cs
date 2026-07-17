using BLL_Ecommerce.Interfaces;
using DAL_Ecommerce;
using DTO_Ecommerce;
using System.Collections.Generic;

namespace BLL_Ecommerce.Services
{
    public class ProductService : IProductService
    {
        DAL_Products ProductCategory = new DAL_Products();


    public  List<sp_SearchProduct_Result> sp_SearchProduct(sp_SearchProduct_Param spParam)
    {
            return ProductCategory.sp_SearchProduct(spParam);
    }


        public List<sp_GetProductFileById_Result> sp_GetProductFileById(int ProductId, bool IncludeInactive)
        {
            return ProductCategory.sp_GetProductFileById(ProductId, IncludeInactive);
        }

        public sp_GetProductFileContent_Result GetProductFileContent(int id)
        {
            return ProductCategory.GetProductFileContent(id);
        }

        public sp_GetProductById_Result sp_GetProductById(int id)
        {
            return ProductCategory.sp_GetProductById(id);
        }

        public int ManageProductFile(sp_ManageProductFile_Param param)
        {
            return ProductCategory.ManageProductFile(param);
        }


    }
}
