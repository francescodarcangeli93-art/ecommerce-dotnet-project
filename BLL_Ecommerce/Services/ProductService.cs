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
    public class ProductService : IProductService
    {
        DAL_Products ProductCategory = new DAL_Products();


    public  List<sp_SearchProduct_Result> sp_SearchProduct(sp_SearchProduct_Param spParam)
    {
            return ProductCategory.sp_SearchProduct(spParam);
    }


    }
}
