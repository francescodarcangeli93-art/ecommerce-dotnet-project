using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_Ecommerce.Attributes
{
    public class DbTableAttribute : Attribute
    {

        private string _tableName;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullName"></param>
        public DbTableAttribute(string fullName)
        {
            _tableName = fullName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="schemaName"></param>
        /// <param name="tableName"></param>
        public DbTableAttribute(string schemaName, string tableName) : this(string.Format("{0}.{1}", schemaName, tableName))
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public string TableName
        {
            get
            {
                return _tableName;
            }
        }
    }
}
