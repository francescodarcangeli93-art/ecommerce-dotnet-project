using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_Ecommerce.DataModels
{
    public interface IRecord
    {
        void Fill(IDataReader reader);
    }
}
