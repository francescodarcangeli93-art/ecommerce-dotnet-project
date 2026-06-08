using DTO_Ecommerce;
using DTO_Ecommerce.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_Ecommerce
{
    public class DAL_LoginUser
    {  
       public List<sp_LoginUser_Result> LoginUser(sp_LoginUser_Param spParam)
        {
            string spName = "dbo.sp_LoginUser";

            List<sp_LoginUser_Result> retVal = new List<sp_LoginUser_Result>();
            DataTable dtUser = new DataTable();

            try
            {
                using (SqlConnection conn = DbConnectionFactory.CreateConnection())
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Username", spParam.Username));
                    cmd.Parameters.Add(new SqlParameter("@Password", spParam.Password));

                    conn.Open();

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtUser);
                    }

                    if (dtUser.Rows.Count > 0)
                    {
                        retVal = dtUser.DataTableToList<sp_LoginUser_Result>();
                       
                    }
                }
            }
            catch (Exception ex) 
            {
                throw ex;
            }
            return retVal;

        }
    }
}
