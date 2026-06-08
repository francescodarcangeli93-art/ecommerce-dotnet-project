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
    public class Ship
    {
        public List<sp_GetShipmentStatusById_Result> sp_GetShipmentStatusById(sp_GetShipmentStatusById_Param spParam)
        {
            string spName = "dbo.sp_GetShipmentStatusById";

            List<sp_GetShipmentStatusById_Result> retVal = new List<sp_GetShipmentStatusById_Result> ();
            DataTable dtShipmentStatus = new DataTable ();    

           try
            {
                using (SqlConnection conn = DbConnectionFactory.CreateConnection())
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@Id", spParam.Id));

                    conn.Open();

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtShipmentStatus);
                    }
                    if (dtShipmentStatus.Rows.Count > 0)
                    {
                        retVal = dtShipmentStatus.DataTableToList<sp_GetShipmentStatusById_Result>();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retVal;
        }

        public List<sp_GetShippingCompanyById_Result> sp_GetShippingCompanyById (sp_GetShippingCompanyById_Param spParam)
        {
            string spName = "dbo.sp_GetShippingCompanyById";

            List<sp_GetShippingCompanyById_Result> retVal = new List<sp_GetShippingCompanyById_Result>();
            DataTable dtShipmentStatus = new DataTable();

            try
            {
                using (SqlConnection conn = DbConnectionFactory.CreateConnection())
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@Id", spParam.Id));

                    conn.Open();

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtShipmentStatus);
                    }
                    if (dtShipmentStatus.Rows.Count > 0)
                    {
                        retVal = dtShipmentStatus.DataTableToList<sp_GetShippingCompanyById_Result>();
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
