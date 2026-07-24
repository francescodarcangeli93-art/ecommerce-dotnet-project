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
                    cmd.Parameters.Add(new SqlParameter("@BasedUnitPriceDa", (object)spParam.BasedUnitPriceDa ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@BasedUnitPriceA", (object)spParam.BasedUnitPriceA ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@Package", (object)spParam.Package ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@IsDiscontinued", (object)spParam.IsDiscontinued ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@CategoryId", (object)spParam.CategoryId ?? DBNull.Value));
                

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

        public List<sp_GetProductFileById_Result> sp_GetProductFileById(int ProductId, bool IncludeInactive, string FileType)
        {
            string spName = "dbo.sp_GetProductFileById";

            List<sp_GetProductFileById_Result> retVal = new List<sp_GetProductFileById_Result>();
            DataTable dtFiles = new DataTable();

            try
            {
                using (SqlConnection conn = DbConnectionFactory.CreateConnection())
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ProductId", ProductId));
                    cmd.Parameters.Add(new SqlParameter("@IncludeInactive", IncludeInactive));
                    cmd.Parameters.AddWithValue("@FileType", (object)FileType ?? DBNull.Value);

                    // Apertura connessione sincrona
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dtFiles.Load(reader);
                    }

                    if (dtFiles.Rows.Count > 0)
                    {
                        retVal = dtFiles.DataTableToList<sp_GetProductFileById_Result>();
                    }
                }
            }
            catch (Exception)
            {
                throw; // "throw;" preserva lo Stack Trace completo dell'eccezione originale
            }

            return retVal;
        }

        public sp_GetProductFileContent_Result GetProductFileContent(int id)
        {
            string spName = "dbo.sp_GetProductFileContent";
            sp_GetProductFileContent_Result retVal = new sp_GetProductFileContent_Result();
            DataTable dt = new DataTable();

            using (SqlConnection conn = DbConnectionFactory.CreateConnection())
            using (SqlCommand cmd = new SqlCommand(spName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Id", id));
                conn.Open();

                using (var da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }

                if (dt.Rows.Count > 0)
                {
                    retVal = dt.Rows[0].ToObject<sp_GetProductFileContent_Result>();
                }
            }

            return retVal;
        }

        public sp_GetProductById_Result sp_GetProductById (int id)
        {
            string spName = "dbo.sp_GetProductById";
            sp_GetProductById_Result retVal = new sp_GetProductById_Result();
            DataTable product = new DataTable();

            using (SqlConnection conn = DbConnectionFactory.CreateConnection())
            using (SqlCommand cmd = new SqlCommand(spName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdProduct", id));
                conn.Open();

                using (var da = new SqlDataAdapter(cmd))
                {
                    da.Fill(product);
                }

                if (product.Rows.Count > 0)
                {
                    retVal = product.Rows[0].ToObject<sp_GetProductById_Result>();
                }
            }

            return retVal;
        }

        public int ManageProductFile(sp_ManageProductFile_Param param)
        {
            string spName = "dbo.sp_ManageProductFile";
            int resultId = 0;

            using (SqlConnection conn = DbConnectionFactory.CreateConnection())
            using (SqlCommand cmd = new SqlCommand(spName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Mappatura dei parametri con gestione dei valori NULL
                cmd.Parameters.Add("@Operation", SqlDbType.Char, 1).Value = (object)param.Operation ?? DBNull.Value;
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = (object)param.Id ?? DBNull.Value;
                cmd.Parameters.Add("@ProductId", SqlDbType.Int).Value = (object)param.ProductId ?? DBNull.Value;
                cmd.Parameters.Add("@FileType", SqlDbType.NVarChar, 20).Value = (object)param.FileType ?? DBNull.Value;
                cmd.Parameters.Add("@FileName", SqlDbType.NVarChar, 150).Value = (object)param.FileName ?? DBNull.Value;
                cmd.Parameters.Add("@FileExtension", SqlDbType.NVarChar, 10).Value = (object)param.FileExtension ?? DBNull.Value;
                cmd.Parameters.Add("@MimeType", SqlDbType.NVarChar, 100).Value = (object)param.MimeType ?? DBNull.Value;

                // Per il tipo VARBINARY(MAX) in ADO.NET viene usato SqlDbType.VarBinary e dimensione -1
                cmd.Parameters.Add("@FileContent", SqlDbType.VarBinary, -1).Value = (object)param.FileContent ?? DBNull.Value;

                cmd.Parameters.Add("@FileSizeBytes", SqlDbType.BigInt).Value = (object)param.FileSizeBytes ?? DBNull.Value;
                cmd.Parameters.Add("@Title", SqlDbType.NVarChar, 100).Value = (object)param.Title ?? DBNull.Value;
                cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 250).Value = (object)param.Description ?? DBNull.Value;
                cmd.Parameters.Add("@IsMain", SqlDbType.Bit).Value = (object)param.IsMain ?? DBNull.Value;
                cmd.Parameters.Add("@DisplayOrder", SqlDbType.Int).Value = (object)param.DisplayOrder ?? DBNull.Value;
                cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = (object)param.IsActive ?? DBNull.Value;

                try
                {
                    conn.Open();

                    // Se è un Inserimento, recuperiamo l'ID generato tramite ExecuteScalar
                    if (param.Operation != null && param.Operation.ToUpper() == "I")
                    {
                        object scalarResult = cmd.ExecuteScalar();
                        if (scalarResult != null && scalarResult != DBNull.Value)
                        {
                            resultId = Convert.ToInt32(scalarResult);
                        }
                    }
                    else
                    {
                        
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }

            return resultId;
        }

        public sp_GetImageFileById_Result sp_GetImageFileById(int id, int productId)
        {
            string spName = "dbo.sp_GetImagesFileById";
            sp_GetImageFileById_Result retVal = new sp_GetImageFileById_Result();
            DataTable dt = new DataTable();

            using (SqlConnection conn = DbConnectionFactory.CreateConnection())
            using (SqlCommand cmd = new SqlCommand(spName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Parametri richiesti dalla Stored Procedure sp_GetImagesFileById
                cmd.Parameters.Add(new SqlParameter("@Id", id));
                cmd.Parameters.Add(new SqlParameter("@ProductId", productId));

                conn.Open();

                using (var da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }

                if (dt.Rows.Count > 0)
                {
                    retVal = dt.Rows[0].ToObject<sp_GetImageFileById_Result>();
                }
            }

            return retVal;
        }

        public List<sp_GetListProductCategoryProduct_Result> sp_GetListProductCategoryProduct()
        {
            string spName = "dbo.sp_GetListProductCategoryProduct";

            List<sp_GetListProductCategoryProduct_Result> retVal = new List<sp_GetListProductCategoryProduct_Result>();
            DataTable dtProducts = new DataTable();

            try
            {
                using (SqlConnection conn = DbConnectionFactory.CreateConnection())
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                   


                    // Apertura connessione sincrona
                    conn.Open();


                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dtProducts.Load(reader);
                    }

                    if (dtProducts.Rows.Count > 0)
                    {
                        retVal = dtProducts.DataTableToList<sp_GetListProductCategoryProduct_Result>();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex; // Mantiene intatto lo Stack Trace dell'errore originale
            }

            return retVal;
        }

        public List<sp_GetListSupplierProduct_Result> sp_GetListSupplierProduct()
        {
            string spName = "dbo.sp_GetListSupplierProduct";

            List<sp_GetListSupplierProduct_Result> retVal = new List<sp_GetListSupplierProduct_Result>();
            DataTable dtProducts = new DataTable();

            try
            {
                using (SqlConnection conn = DbConnectionFactory.CreateConnection())
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {



                    // Apertura connessione sincrona
                    conn.Open();


                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dtProducts.Load(reader);
                    }

                    if (dtProducts.Rows.Count > 0)
                    {
                        retVal = dtProducts.DataTableToList<sp_GetListSupplierProduct_Result>();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex; // Mantiene intatto lo Stack Trace dell'errore originale
            }

            return retVal;
        }

        /*===================================================================================================================================================
           MANAGE PRODUCT
         ====================================================================================================================================================*/

        public void sp_ManageProduct(sp_ManageProduct_Param param)
        {
            string spName = "dbo.sp_ManageProduct";

            using (SqlConnection conn = DbConnectionFactory.CreateConnection())
            using (SqlCommand cmd = new SqlCommand(spName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@Operation", SqlDbType.Char, 1).Value = param.Operation;
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = (object)param.Id ?? DBNull.Value;
                cmd.Parameters.Add("@ProductName", SqlDbType.NVarChar, 50).Value = (object)param.ProductName ?? DBNull.Value;
                cmd.Parameters.Add("@SupplierId", SqlDbType.Int).Value = (object)param.SupplierId ?? DBNull.Value;
                cmd.Parameters.Add("@CategoryId", SqlDbType.Int).Value = (object)param.CategoryId ?? DBNull.Value;

                var priceParam = cmd.Parameters.Add("@BasedUnitPrice", SqlDbType.Decimal);
                priceParam.Precision = 12;
                priceParam.Scale = 2;
                priceParam.Value = (object)param.BasedUnitPrice ?? DBNull.Value;

                cmd.Parameters.Add("@Package", SqlDbType.NVarChar, 30).Value = (object)param.Package ?? DBNull.Value;
                cmd.Parameters.Add("@IsDiscontinued", SqlDbType.Bit).Value = (object)param.IsDiscontinued ?? DBNull.Value;
                cmd.Parameters.Add("@VisibleInHP", SqlDbType.Bit).Value = (object)param.VisibleInHP ?? DBNull.Value;

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery(); // ignora SCOPE_IDENTITY(), va bene comunque anche con Insert
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }
        }

    }
}
