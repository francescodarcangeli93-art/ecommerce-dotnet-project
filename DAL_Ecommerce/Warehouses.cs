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
                throw ex; // Mantiene intatto lo Stack Trace dell'errore originale
            }

            return retVal;
        }

        public void ManageWarehouse(sp_ManageWarehouse_Param param)
        {
            string spName = "dbo.sp_ManageWarehouse";



            using (SqlConnection conn = DbConnectionFactory.CreateConnection())
            using (SqlCommand cmd = new SqlCommand(spName, conn))
            {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Mappatura dei parametri con gestione dei valori NULL
                    cmd.Parameters.Add("@Operation", SqlDbType.Char, 1).Value = param.Operation;
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = (object)param.Id ?? DBNull.Value;
                    cmd.Parameters.Add("@Type", SqlDbType.NVarChar, 10).Value = (object)param.Type ?? DBNull.Value;
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 100).Value = (object)param.Name ?? DBNull.Value;
                    cmd.Parameters.Add("@Address", SqlDbType.NVarChar, 255).Value = (object)param.Address ?? DBNull.Value;
                    cmd.Parameters.Add("@PostalCode", SqlDbType.NChar, 5).Value = (object)param.PostalCode ?? DBNull.Value;
                    cmd.Parameters.Add("@City", SqlDbType.NVarChar, 100).Value = (object)param.City ?? DBNull.Value;
                    cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = (object)param.IsActive ?? DBNull.Value;

                    try
                    {
                        conn.Open();
                   
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
               
                        throw new Exception(ex.Message, ex);
                    }
            }
            
        }

        public sp_GetWarehouseById_Result sp_GetWarehouseById (int? id)
        {
            string spName = "dbo.sp_GetWarehouseById";

            //sp_GetWarehouseById_Param param = new sp_GetWarehouseById_Param
            //{
            //    Id = id
            //};

            sp_GetWarehouseById_Result Warehouse = new sp_GetWarehouseById_Result();

            try
            {
                using (SqlConnection conn = DbConnectionFactory.CreateConnection())
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Id", id));

                
                    conn.Open();

                    // Esegue la stored procedure in modo sincrono
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                      
                        if (reader.Read())
                        {
                            Warehouse = new sp_GetWarehouseById_Result
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Type = reader["Type"].ToString(),
                                Name = reader["Name"].ToString(),
                                Address = reader["Address"].ToString(),
                                PostalCode = reader["PostalCode"].ToString(),
                                City = reader["City"].ToString(),
                                IsActive = (bool)reader["IsActive"]
                                
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return Warehouse;
        }

        public List<sp_GetWarehouseMovementsById_Result> sp_GetWarehouseMovementsById(int WarehouseId)
        {
            string spName = "dbo.sp_GetWarehouseMovementsById";

            List<sp_GetWarehouseMovementsById_Result> retVal = new List<sp_GetWarehouseMovementsById_Result>();
            DataTable dtMovements = new DataTable();

            try
            {
                using (SqlConnection conn = DbConnectionFactory.CreateConnection())
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@WarehouseId", WarehouseId));

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dtMovements.Load(reader);
                    }

                    if (dtMovements.Rows.Count > 0)
                    {
                        retVal = dtMovements.DataTableToList<sp_GetWarehouseMovementsById_Result>();
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
