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
    public class CountryService : ICountryService
    {
        DAL_Country country =  new DAL_Country();
        public  List<sp_GetListCountry_Result> GetListCountry()
        {
           return  country.ListCountry();
        }

    }
}
