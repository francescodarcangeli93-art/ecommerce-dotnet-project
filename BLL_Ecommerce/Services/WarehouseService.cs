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
    public class WarehouseService : IWarehousesService
    {
        DAL_Warehouses Warehouses = new DAL_Warehouses ();

        public  List<sp_SearchWarehouses_Result> SearchWarehouses(sp_SearchWarehouses_Param param)
        {

          return  Warehouses.sp_SearchWarehouses(param);
        }
    }
}
