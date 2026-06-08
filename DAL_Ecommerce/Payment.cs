using DTO_Ecommerce;
using DTO_Ecommerce.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL_Ecommerce
{
    public class Payment
    {
        public List<sp_GetPaymentTypeById_Result> GetPaymentTypeById(sp_GetPaymentTypeById_Param spParam)
        {
            string spName = "sp_GetPaymentTypeById";

            List<sp_GetPaymentTypeById_Result> retVal = new List<sp_GetPaymentTypeById_Result>();
            DataTable dtPaymentType = new DataTable();

            try
            {
                using (SqlConnection conn = DbConnectionFactory.CreateConnection())
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Id", spParam.Id));
                    cmd.Parameters.Add(new SqlParameter("@FilterType", spParam.FilterType));

                    conn.Open();

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtPaymentType);
                    }

                    if (dtPaymentType.Rows.Count > 0)
                    {
                        retVal = dtPaymentType.DataTableToList<sp_GetPaymentTypeById_Result>();

                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }


        public List<sp_GetPaymentStatusById_Result> GetPaymentStatusById(sp_GetPaymentStatusById_Param spParam)
        {
            string spName = "GetPaymentStatusById";

            List<sp_GetPaymentStatusById_Result> retVal = new List<sp_GetPaymentStatusById_Result>();
            DataTable dtPaymentType = new DataTable();

            try
            {
                using (SqlConnection conn = DbConnectionFactory.CreateConnection())
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
  
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Id", spParam.Id));
                    cmd.Parameters.Add(new SqlParameter("@Code", spParam.Code));
                    cmd.Parameters.Add(new SqlParameter("@Description", spParam.Description));

                    conn.Open();
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtPaymentType);
                    }

                    if (dtPaymentType.Rows.Count > 0)
                    {
                        retVal = dtPaymentType.DataTableToList<sp_GetPaymentStatusById_Result>();
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
