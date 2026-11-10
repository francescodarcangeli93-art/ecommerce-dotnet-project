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

        public void sp_WarehouseMoveProducts(sp_WarehouseMoveProducts_Param param)
        {
            WarehousesMovements.sp_WarehouseMoveProducts(param);
        }

        public List<sp_GetMovementReasonList_Result> sp_GetMovementReasonList()
        {
            return WarehousesMovements.sp_GetMovementReasonList();
        }

        public List<sp_GetAllWareHouseList_Result> sp_GetAllWareHouseList()
        {
            return WarehousesMovements.sp_GetAllWareHouseList();
        }

        public sp_GetWarehousesMovementsById_Result sp_GetWarehousesMovementsById(int warehouseMovementId)
        {
            return WarehousesMovements.sp_GetWarehousesMovementsById(warehouseMovementId);
        }

        public void sp_UpdateWarehouseMovementNotes(sp_UpdateWarehouseMovementNotes_Param param)
        {
             WarehousesMovements.sp_UpdateWarehouseMovementNotes(param);
        }
    }
}
