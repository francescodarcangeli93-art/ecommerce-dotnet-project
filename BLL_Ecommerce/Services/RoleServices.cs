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

            retVal = RoleDal.ListUserRole_Results();

            return retVal;
        }
    }
}
