using DTO_Ecommerce;
using System.Collections.Generic;

namespace BLL_Ecommerce.Interfaces
{
    public interface IProductService
    {
        List<sp_SearchProduct_Result> sp_SearchProduct(sp_SearchProduct_Param spParam);

    }
}