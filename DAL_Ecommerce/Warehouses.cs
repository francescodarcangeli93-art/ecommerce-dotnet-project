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
    public class DAL_Warehouses
    {
        public List<sp_SearchWarehouses_Result> sp_SearchWarehouses(sp_SearchWarehouses_Param spParam)
        {
            string spName = "dbo.sp_SearchWarehouses";

            List<sp_SearchWarehouses_Result> retVal = new List<sp_SearchWarehouses_Result>();
            DataTable dtWarehouses = new DataTable(); 

            try
            {
                using (SqlConnection conn = DbConnectionFactory.CreateConnection())
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Name", (object)spParam.Name ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@Address", (object)spParam.Address ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@City", (object)spParam.City ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@PostalCode", (object)spParam.PostalCode ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@Type", (object)spParam.Type ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@IsActive", (object)spParam.IsActive ?? DBNull.Value));

                    // Apertura connessione sincrona
                    conn.Open();

                    
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dtWarehouses.Load(reader);
                    }

                    if (dtWarehouses.Rows.Count > 0)
                    {
                        retVal = dtWarehouses.DataTableToList<sp_SearchWarehouses_Result>();
                    }
                }
            }
            catch (Exception ex)
            {
                throw; // Mantiene intatto lo Stack Trace dell'errore originale
            }

            return retVal;
        }

    }
}
