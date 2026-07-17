using DTO_Ecommerce;
using System.Collections.Generic;

namespace BLL_Ecommerce.Interfaces
{
    public interface IProductService
    {
        List<sp_SearchProduct_Result> sp_SearchProduct(sp_SearchProduct_Param spParam);

        List<sp_GetProductFileById_Result> sp_GetProductFileById(int ProductId, bool IncludeInactive);

        sp_GetProductFileContent_Result GetProductFileContent(int id);


        sp_GetProductById_Result sp_GetProductById(int id);

        int ManageProductFile(sp_ManageProductFile_Param param);
    }
}