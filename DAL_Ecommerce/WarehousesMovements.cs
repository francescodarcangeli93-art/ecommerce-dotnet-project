using DTO_Ecommerce;
using DTO_Ecommerce.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

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
                    cmd.Parameters.Add(new SqlParameter("@Description", (object)spParam.Description ?? DBNull.Value));

                    cmd.Parameters.Add(new SqlParameter("@PageNumber", (object)spParam.PageNumber ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@PageSize", (object)spParam.PageSize ?? DBNull.Value));
                

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
                throw ; 
            }

            return retVal;
        }

        public void sp_WarehouseMoveProducts(sp_WarehouseMoveProducts_Param param)
        {
            string spName = "dbo.sp_WarehouseMoveProducts";

            using (SqlConnection conn = DbConnectionFactory.CreateConnection())
            using (SqlCommand cmd = new SqlCommand(spName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Mappatura corretta con i parametri della Stored Procedure
                cmd.Parameters.Add("@MovementReasonId", SqlDbType.SmallInt).Value = param.MovementReasonId;
                cmd.Parameters.Add("@ProductId", SqlDbType.Int).Value = param.ProductId;
                cmd.Parameters.Add("@QtyToTransfer", SqlDbType.Int).Value = param.QtyToTransfer;
                cmd.Parameters.Add("@WarehouseId", SqlDbType.Int).Value = param.WarehouseId;
                cmd.Parameters.Add("@Notes", SqlDbType.VarChar).Value = param.Notes;
                
                // Parametro opzionale (può essere NULL se si tratta di carico o scarico singolo)
                cmd.Parameters.Add("@DestinationWarehouseId", SqlDbType.Int).Value = (object)param.DestinationWarehouseId ?? DBNull.Value;

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {

                    throw new WarehouseOperationException($"Errore nella movimentazione di magazzino: {ex.Message}", ex);
                }
            }
        }

        public List<sp_GetMovementReasonList_Result> sp_GetMovementReasonList()
        {
            string spName = "dbo.sp_GetMovementReasonList";

            List<sp_GetMovementReasonList_Result> retVal = new List<sp_GetMovementReasonList_Result>();
            DataTable dtMovementReasonList = new DataTable();

            try
            {
                using (SqlConnection conn = DbConnectionFactory.CreateConnection())
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                 
                    // Apertura connessione sincrona
                    conn.Open();


                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dtMovementReasonList.Load(reader);
                    }

                    if (dtMovementReasonList.Rows.Count > 0)
                    {
                        retVal = dtMovementReasonList.DataTableToList<sp_GetMovementReasonList_Result>();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retVal;

        }

        public List<sp_GetAllWareHouseList_Result> sp_GetAllWareHouseList()
        {
            string spName = "dbo.sp_GetAllWareHouseList";

            List<sp_GetAllWareHouseList_Result> retVal = new List<sp_GetAllWareHouseList_Result>();
            DataTable dtGetAllWareHouse = new DataTable();

            try
            {
                using (SqlConnection conn = DbConnectionFactory.CreateConnection())
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {

                    // Apertura connessione sincrona
                    conn.Open();


                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dtGetAllWareHouse.Load(reader);
                    }

                    if (dtGetAllWareHouse.Rows.Count > 0)
                    {
                        retVal = dtGetAllWareHouse.DataTableToList<sp_GetAllWareHouseList_Result>();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retVal;

        }

        public sp_GetWarehousesMovementsById_Result sp_GetWarehousesMovementsById(int warehouseMovementId)
        {
            string spName = "dbo.sp_GetWarehousesMovementsById";

            // Inizializziamo a null
            sp_GetWarehousesMovementsById_Result retVal = null;
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection conn = DbConnectionFactory.CreateConnection())
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                
                    cmd.CommandType = CommandType.StoredProcedure;

             
                    cmd.Parameters.AddWithValue("@WarehouseMovementId", warehouseMovementId);

                    // Apertura connessione
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }

                   
                    if (dt.Rows.Count > 0)
                    {
                        
                        retVal = dt.DataTableToList<sp_GetWarehousesMovementsById_Result>().FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
              
                throw ex;
            }

            return retVal;
        }

        public void sp_UpdateWarehouseMovementNotes(sp_UpdateWarehouseMovementNotes_Param param)
        {
            string spName = "dbo.sp_UpdateWarehouseMovementNotes";

            using (SqlConnection conn = DbConnectionFactory.CreateConnection())
            using (SqlCommand cmd = new SqlCommand(spName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Mappatura corretta con i parametri della Stored Procedure
                cmd.Parameters.Add("@MovementId", SqlDbType.Int).Value = param.MovementId;
                cmd.Parameters.Add("@Notes", SqlDbType.VarChar).Value = param.Notes;
               
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {

                    throw new WarehouseOperationException($"Impossibile modificare la nota: {ex.Message}", ex);
                }
            }
        }

    }
}
