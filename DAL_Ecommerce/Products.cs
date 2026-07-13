using DTO_Ecommerce;
using DTO_Ecommerce.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL_Ecommerce
{
    public class DAL_Products
    {
        public List<sp_SearchProduct_Result> sp_SearchProduct(sp_SearchProduct_Param spParam)
        {
            string spName = "dbo.sp_SearchProduct";

            List<sp_SearchProduct_Result> retVal = new List<sp_SearchProduct_Result>();
            DataTable dtProducts = new DataTable(); 

            try
            {
                using (SqlConnection conn = DbConnectionFactory.CreateConnection())
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ProductName", (object)spParam.ProductName ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@SupplierId", (object)spParam.SupplierId ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@BasedUnitPrice", (object)spParam.BasedUnitPrice ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@Package", (object)spParam.Package ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@IsDiscontinued", (object)spParam.IsDiscontinued ?? DBNull.Value));
                

                    // Apertura connessione sincrona
                    conn.Open();

                    
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dtProducts.Load(reader);
                    }

                    if (dtProducts.Rows.Count > 0)
                    {
                        retVal = dtProducts.DataTableToList<sp_SearchProduct_Result>();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex; // Mantiene intatto lo Stack Trace dell'errore originale
            }

            return retVal;
        }

       
    }
}
