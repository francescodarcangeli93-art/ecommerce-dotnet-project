using BLL_Ecommerce.Interfaces;
using DAL_Ecommerce;
using DTO_Ecommerce;
using System;
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

    public  void ManagePriceList(sp_ManagePriceList_Param param)
    {
            if (string.IsNullOrWhiteSpace(param.Operation))
                throw new ArgumentException("Operation è obbligatorio (I, U o D).", nameof(param.Operation));

            ThePriceList.ManagePriceList(param);
    }

        public sp_GetPriceListById_Result GetPriceListById(sp_GetPriceListById_Param Param)
        {

       

            return ThePriceList.GetPriceListById(Param);
        }

        public int? ManagePriceListItem(sp_ManagePriceListItem_Param spParam)
        {
            if (spParam == null) throw new ArgumentNullException(nameof(spParam));


            return ThePriceList.ManagePriceListItem(spParam);
        }

        public sp_GetPriceListItemById_Result GetPriceListItemById(int? id)
        {
            if (id == null) throw new ArgumentNullException();

          return  ThePriceList.GetPriceListItemById(id);
        }
    }
}
