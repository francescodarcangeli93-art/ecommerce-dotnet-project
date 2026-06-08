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
    public class ListUserRole
    {
        public List<sp_GetListUserRole_Result> ListUserRole_Results()
        {
            string spName = "dbo.sp_GetListUserRole";

            List<sp_GetListUserRole_Result> retVal = new List<sp_GetListUserRole_Result>();
            DataTable dtUser = new DataTable();

            try
            {
                //  cmd.Parameters.Add(new SqlParameter("@Username", spParam.Username));
                using (SqlConnection conn = DbConnectionFactory.CreateConnection())
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtUser);
                    }

                    if (dtUser.Rows.Count > 0)
                    {
                        retVal = dtUser.DataTableToList<sp_GetListUserRole_Result>();
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
