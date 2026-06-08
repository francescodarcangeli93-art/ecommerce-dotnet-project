using DTO_Ecommerce;
using DTO_Ecommerce.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DAL_Ecommerce
{
  
    public class DAL_Customer
    {
        public int sp_ManageCustomer(sp_ManageCustomer_Param spParam)
        {
            string spName = "dbo.sp_ManageCustomer";

            using (SqlConnection conn = DbConnectionFactory.CreateConnection())
            using (SqlCommand cmd = new SqlCommand(spName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@Operation", spParam.Operation));
                cmd.Parameters.Add(new SqlParameter("@Id", (object)spParam.Id ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@FirstName", (object)spParam.FirstName ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@LastName", (object)spParam.LastName ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@City", (object)spParam.City ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@Country", (object)spParam.Country ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@Phone", (object)spParam.Phone ?? DBNull.Value));

                // Definisci il parametro output
                SqlParameter outParam = new SqlParameter("@CustomerIdInserted", SqlDbType.Int);
                outParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outParam);

                try
                {
                    conn.Open();
                  
                    cmd.ExecuteNonQuery();


                    int idGenerato = outParam.Value != DBNull.Value ? (int)outParam.Value : 0;
                    return idGenerato;
                }
                catch (SqlException)
                {
                    throw; 
                }
            }
        }

    }
}
