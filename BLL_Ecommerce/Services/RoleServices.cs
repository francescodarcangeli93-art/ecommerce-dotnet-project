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

   public class RoleServices : IRoleService
    {

        public ListUserRole RoleDal = new ListUserRole();


        public List<sp_GetListUserRole_Result> UserRoleList()
        {
            List<sp_GetListUserRole_Result> retVal = new List<sp_GetListUserRole_Result>();

            return RoleDal.ListUserRole_Results(); 
        }

        public List<sp_GetListProduct_Result> sp_GetListProduct()
        {
            return RoleDal.sp_GetListProduct();
        }

        public List<sp_GetListProductCategory_Result> ListProductCategory()
        {
            return RoleDal.sp_GetListProductCategory();
        }

        public List<sp_GetListSupplier_Result> GetListSupplier()
        {
            return RoleDal.sp_GetListSupplier();
        }
    }
}
