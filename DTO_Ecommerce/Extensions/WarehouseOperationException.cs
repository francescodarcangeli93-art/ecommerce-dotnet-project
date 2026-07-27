using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_Ecommerce.Extensions
{
    public class WarehouseOperationException : Exception
    {
        public WarehouseOperationException(string message, Exception innerException)
         : base(message, innerException) { }
    }
}
