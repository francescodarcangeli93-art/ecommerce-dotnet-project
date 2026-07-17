using BLL_Ecommerce.Interfaces;
using DAL_Ecommerce;
using DTO_Ecommerce;
using System.Collections.Generic;

namespace BLL_Ecommerce.Services
{
    public class WarehousesMovementsService : IWarehousesMovementsService
    {
        DAL_WarehousesMovements WarehousesMovements = new DAL_WarehousesMovements();

        public List<sp_SearchWarehousesMovements_Result> sp_SearchWarehousesMovements(sp_SearchWarehousesMovements_Param spParam)
        {
            return WarehousesMovements.sp_SearchWarehousesMovements(spParam);
        }



    }
}
