using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_Ecommerce.DataModels
{
    public interface ISubRecord<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        bool IsSameRecord(T record, IDataReader reader);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <param name="reader"></param>
        void FillSubRecord(T record, IDataReader reader);
    }   
}
