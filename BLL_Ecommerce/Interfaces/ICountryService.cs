using DTO_Ecommerce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Ecommerce.Interfaces
{
    public interface ICountryService
    {
        List<sp_GetListCountry_Result> GetListCountry();
    }
}
