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
    public class DAL_ProductCategory
    {
        public List<sp_SearchProductCategory_Result> sp_SearchProductCategory(sp_SearchProductCategory_Param spParam)
        {
            string spName = "dbo.sp_SearchProductCategory";

            List<sp_SearchProductCategory_Result> retVal = new List<sp_SearchProductCategory_Result>();
            DataTable dtWarehouses = new DataTable(); 

            try
            {
                using (SqlConnection conn = DbConnectionFactory.CreateConnection())
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Name", (object)spParam.Name ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@IsActive", (object)spParam.IsActive ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@DisplayOrder", (object)spParam.DisplayOrder ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@Description", (object)spParam.Description ?? DBNull.Value));
                  
                    // Apertura connessione sincrona
                    conn.Open();

                    
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dtWarehouses.Load(reader);
                    }

                    if (dtWarehouses.Rows.Count > 0)
                    {
                        retVal = dtWarehouses.DataTableToList<sp_SearchProductCategory_Result>();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex; 
            }

            return retVal;
        }



        public sp_GetProductCategoryById_Result GetProductCategoryById(int? id)
        {
            string spName = "dbo.sp_GetProductCategoryById";

            sp_GetProductCategoryById_Result ProductCategory = new sp_GetProductCategoryById_Result();

            try
            {
                using (SqlConnection conn = DbConnectionFactory.CreateConnection())
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Id", id));

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ProductCategory = new sp_GetProductCategoryById_Result
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"] == DBNull.Value ? null : reader["Description"].ToString(),
                                DisplayOrder = Convert.ToInt32(reader["DisplayOrder"]),
                                IsActive = Convert.ToBoolean(reader["IsActive"]),
                                CreatedDate = Convert.ToDateTime(reader["CreatedDate"])
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ProductCategory;
        }

        public void ManageProductCategory(sp_ManageProductCategory_Param param)
        {
            string spName = "dbo.sp_ManageProductCategory";

            using (SqlConnection conn = DbConnectionFactory.CreateConnection())
            using (SqlCommand cmd = new SqlCommand(spName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Mappatura dei parametri con gestione dei valori NULL
                cmd.Parameters.Add("@Operation", SqlDbType.Char, 1).Value = param.Operation;
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = (object)param.Id ?? DBNull.Value;
                cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 100).Value = (object)param.Name ?? DBNull.Value;
                cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 250).Value = (object)param.Description ?? DBNull.Value;
                cmd.Parameters.Add("@DisplayOrder", SqlDbType.Int).Value = (object)param.DisplayOrder ?? DBNull.Value;
                cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = (object)param.IsActive ?? DBNull.Value;
                cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = (object)param.CreatedDate ?? DBNull.Value;

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


    }
}
