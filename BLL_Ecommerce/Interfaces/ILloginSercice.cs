using DTO_Ecommerce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Ecommerce.Interfaaces
{
    public interface ILoginServices
    {
        List<sp_LoginUser_Result> LoginUser(sp_LoginUser_Param spParam);
    }

    



}
