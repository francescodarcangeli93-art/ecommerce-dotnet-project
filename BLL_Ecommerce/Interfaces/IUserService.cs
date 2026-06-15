using DAL_Ecommerce;
using DTO_Ecommerce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Ecommerce.Interfaces
{
    public interface IUserService
    {
        List<sp_SearchUser_Result> SearchUser (sp_SearchUser_Param param);

        sp_GetUserById_Result GetUserById(sp_GetUserById_Param spParam);

        sp_GetUserById_Result GetUserById(int? id);

        void sp_ManageUser(sp_ManageUser_Param spParam);

    }
}
