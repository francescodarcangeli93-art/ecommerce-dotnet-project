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
    public class DAL_Supplier
    {
        public List<sp_SearchSupplier_Result> SearchSupplier(sp_SearchSupplier_Param spParam)
        {
            string spName = "dbo.sp_SearchSupplier";

            List<sp_SearchSupplier_Result> retVal = new List<sp_SearchSupplier_Result>();
            DataTable dtSupplier = new DataTable();

            try
            {
                using (SqlConnection conn = DbConnectionFactory.CreateConnection())
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@CompanyName", spParam.CompanyName));
                    cmd.Parameters.Add(new SqlParameter("@ContactName", spParam.ContactName));
                    cmd.Parameters.Add(new SqlParameter("@ContactTitle", spParam.ContactTitle));
                    cmd.Parameters.Add(new SqlParameter("@City", spParam.City));
                    cmd.Parameters.Add(new SqlParameter("@Country", spParam.Country));
                    cmd.Parameters.Add(new SqlParameter("@Phone", spParam.Phone));
                    cmd.Parameters.Add(new SqlParameter("@Fax", spParam.Fax));

                    conn.Open();

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtSupplier);
                    }

                    if (dtSupplier.Rows.Count > 0)
                    {
                        retVal = dtSupplier.DataTableToList<sp_SearchSupplier_Result>();
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
