using DTO_Ecommerce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Ecommerce.Interfaaces
{
    public interface ILogService
    {
        int WriteLogLogin(int? idUser);

        int WriteLogLogout(int? idUser);

        int WriteLogProcessError(int? idUser, string errorMessage);

        int WriteLogProcessWarning(int? idUser, string errorMessage);

        int WriteLogProcessInfo(int? idUser);
    }

    



}
