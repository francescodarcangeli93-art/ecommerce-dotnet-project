using BLL_Ecommerce.Interfaces;
using DAL_Ecommerce;
using DTO_Ecommerce;
using System.Collections.Generic;

namespace BLL_Ecommerce.Services
{
    public class PriceListService : IPriceListService
    {
        DAL_PriceList ThePriceList = new DAL_PriceList();

        public List<sp_SearchPriceList_Result> SearchPriceList(sp_SearchPriceList_Param param)
        {
            return ThePriceList.SearchPriceList(param);
        }



    }
}
