using DTO_Ecommerce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Ecommerce.Interfaces
{
    public interface IPriceListService
    {
         List<sp_SearchPriceList_Result> SearchPriceList(sp_SearchPriceList_Param param);
        
    }

}
