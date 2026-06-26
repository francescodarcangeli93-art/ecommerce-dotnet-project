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
                    cmd.Parameters.Add(new SqlParameter("@CompanyName", (object)spParam.CompanyName ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@ContactName", (object)spParam.ContactName ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@ContactTitle", (object)spParam.ContactTitle ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@City", (object)spParam.City ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@Country", (object)spParam.Country ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@Phone", (object)spParam.Phone ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@Fax", (object)spParam.Fax ?? DBNull.Value));

                    // Apertura connessione sincrona
                    conn.Open();

                    // Lettura sincrona dei dati tramite reader
                    using (SqlDataReader reader = cmd.ExecuteReader())
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

        public sp_GetSupplierById_Result GetSupplierById(int id)
        {
            string spName = "dbo.sp_GetSupplierById";

            sp_GetSupplierById_Param param = new sp_GetSupplierById_Param
            {
                IdSupplier = id
            };

            sp_GetSupplierById_Result Supplier = new sp_GetSupplierById_Result();

            try
            {
                using (SqlConnection conn = DbConnectionFactory.CreateConnection())
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdSupplier", param.IdSupplier));

                    // Versione sincrona dell'apertura della connessione
                    conn.Open();

                    // Esegue la stored procedure in modo sincrono
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Lettura sincrona dei dati
                        if (reader.Read())
                        {
                            Supplier = new sp_GetSupplierById_Result
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                CompanyName = reader["CompanyName"].ToString(),
                                ContactName = reader["ContactName"].ToString(),
                                ContactTitle = reader["ContactTitle"].ToString(),
                                City = reader["City"].ToString(),
                                Country = reader["Country"].ToString(),
                                Phone = reader["Phone"].ToString(),
                                Fax = reader["Fax"].ToString()
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
               
                throw;
            }

            return Supplier;
        }

        public void sp_ManageSupplier(sp_ManageSupplier_Param spParam)
        {
            string spName = "dbo.sp_ManageSupplier";

            using (SqlConnection conn = DbConnectionFactory.CreateConnection())
            using (SqlCommand cmd = new SqlCommand(spName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@Operation", spParam.Operation));
                cmd.Parameters.Add(new SqlParameter("@Id", spParam.Id));
                cmd.Parameters.Add(new SqlParameter("@CompanyName", spParam.CompanyName));
                cmd.Parameters.Add(new SqlParameter("@ContactName", spParam.ContactName));
                cmd.Parameters.Add(new SqlParameter("@ContactTitle", spParam.ContactTitle));
                cmd.Parameters.Add(new SqlParameter("@City", spParam.City));
                cmd.Parameters.Add(new SqlParameter("@Country", spParam.Country));
                cmd.Parameters.Add(new SqlParameter("@Phone", spParam.Phone));
                cmd.Parameters.Add(new SqlParameter("@Fax", spParam.Fax));

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException)
                {
                    throw;
                }
            }
        }

    }
}
