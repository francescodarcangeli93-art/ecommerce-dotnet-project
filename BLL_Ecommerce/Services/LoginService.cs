using BLL_Ecommerce.Interfaaces;
using DAL_Ecommerce;
using DTO_Ecommerce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Ecommerce.Services
{
    public class LoginService : ILoginServices
    {
        DAL_LoginUser loginDal = new DAL_LoginUser();

        public List<sp_LoginUser_Result> LoginUser(sp_LoginUser_Param spParam)
        {
            List<sp_LoginUser_Result> retVal = new List<sp_LoginUser_Result>();

            spParam.Password = EncryptionHelper.Encrypt(spParam.Password);

            retVal = loginDal.LoginUser(spParam);

            return retVal;
        }


    }
}
