using DTO_Ecommerce;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_Ecommerce
{
    public class DAL_Log
    {
        public int WriteLog(ref sp_ManageLog_Param spParam)
        {
            string spName = "sp_ManageLog";

            try
            {
                using (SqlConnection conn = DbConnectionFactory.CreateConnection())
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Adding Input parameter.
                    cmd.Parameters.Add(new SqlParameter("@IdUser", spParam.IdUser));
                    cmd.Parameters.Add(new SqlParameter("@idLogType", spParam.IdLogType));
                    cmd.Parameters.Add(new SqlParameter("@idLogSeverity", spParam.IdLogSeverity));
                    cmd.Parameters.Add(new SqlParameter("@errorMessage", spParam.ErrorMessage));

                    conn.Open();

                    int retVal = cmd.ExecuteNonQuery();

                    return retVal;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
