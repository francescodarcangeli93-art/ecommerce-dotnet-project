using DTO_Ecommerce;
using DTO_Ecommerce.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace DAL_Ecommerce
{
    public class DAL_Country
    {
        public List<sp_GetListCountry_Result> ListCountry()
        {
            string spName = "dbo.sp_GetListCountry";

            List<sp_GetListCountry_Result> retVal = new List<sp_GetListCountry_Result>();
            DataTable dtCountry = new DataTable();

            try
            {
                using (SqlConnection conn = DbConnectionFactory.CreateConnection())
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Apertura connessione sincrona
                    conn.Open();

                    // Lettura sincrona tramite reader
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dtCountry.Load(reader);
                    }
                }

                if (dtCountry.Rows.Count > 0)
                {
                    return dtCountry.DataTableToList<sp_GetListCountry_Result>();
                }
            }
            catch (Exception ex)
            {
                throw; // Mantiene intatto lo Stack Trace dell'errore
            }

            return new List<sp_GetListCountry_Result>();
        }

    }
}
