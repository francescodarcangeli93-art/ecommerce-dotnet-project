using DTO_Ecommerce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Ecommerce.Interfaces
{
    public interface IWarehousesMovementsService
    {
        List<sp_SearchWarehousesMovements_Result> sp_SearchWarehousesMovements(sp_SearchWarehousesMovements_Param spParam);
    }
}