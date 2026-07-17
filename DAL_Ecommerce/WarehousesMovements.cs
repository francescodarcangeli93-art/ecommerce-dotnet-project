using DTO_Ecommerce;
using DTO_Ecommerce.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL_Ecommerce
{
    public class DAL_WarehousesMovements
    {
        public List<sp_SearchWarehousesMovements_Result> sp_SearchWarehousesMovements (sp_SearchWarehousesMovements_Param spParam)
        {
            string spName = "dbo.sp_SearchWarehousesMovements";

            List<sp_SearchWarehousesMovements_Result> retVal = new List<sp_SearchWarehousesMovements_Result>();
            DataTable dtWarehousesMovements = new DataTable(); 

            try
            {
                using (SqlConnection conn = DbConnectionFactory.CreateConnection())
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@StartDate", (object)spParam.StartDate ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@EndDate", (object)spParam.EndDate ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@WarehouseId", (object)spParam.WarehouseId ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@ProductId", (object)spParam.ProductId ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@MovementType", (object)spParam.MovementType ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@MovementReasonId", (object)spParam.MovementReasonId ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@CompanyName", (object)spParam.CompanyName ?? DBNull.Value));

                    // Apertura connessione sincrona
                    conn.Open();

                    
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dtWarehousesMovements.Load(reader);
                    }

                    if (dtWarehousesMovements.Rows.Count > 0)
                    {
                        retVal = dtWarehousesMovements.DataTableToList<sp_SearchWarehousesMovements_Result>();
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
