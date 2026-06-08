using DTO_Ecommerce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Ecommerce.Interfaces
{
    public interface ILRoleService
    {
            List<sp_GetListUserRole_Result> UserRoleList();
    }
}
