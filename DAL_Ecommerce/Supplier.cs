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
        public async Task<List<sp_SearchSupplier_Result>> SearchSupplier(sp_SearchSupplier_Param spParam)
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
                    cmd.Parameters.Add(new SqlParameter("@CompanyName", (object)spParam.CompanyName ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@ContactName", (object)spParam.ContactName ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@ContactTitle", (object)spParam.ContactTitle ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@City", (object)spParam.City ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@Country", (object)spParam.Country ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@Phone", (object)spParam.Phone ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@Fax", (object)spParam.Fax ?? DBNull.Value));

                    // apertura connessione asincrona
                    await conn.OpenAsync();

                    // Lettura asincrona dei dati tramite reader
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        dtSupplier.Load(reader);
                    }

                    if (dtSupplier.Rows.Count > 0)
                    {
                        retVal = dtSupplier.DataTableToList<sp_SearchSupplier_Result>();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return retVal;
        }

    }
}
