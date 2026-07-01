using DTO_Ecommerce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Ecommerce.Interfaces
{
    public interface IWarehousesService
    {
        List<sp_SearchWarehouses_Result> SearchWarehouses(sp_SearchWarehouses_Param param);
        void sp_ManageWarehouses(sp_ManageWarehouse_Param param);
        sp_GetWarehouseById_Result GetWarehouseById(int? id);
        List<sp_GetWarehouseMovementsById_Result> sp_GetWarehouseMovementsById(int WarehouseId);
    }
}