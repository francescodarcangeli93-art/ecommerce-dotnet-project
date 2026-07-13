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
    public class ListUserRole
    {

        /*********************************************************************************************************************
                 CLASSE CHE CONTIENE METODI CHE GESTISCONO I DATI DA INSERIRE NELLE DROPDOWN 
         ********************************************************************************************************************/



        /*=======================================================================================
            1 LISTA RUOLI UTENTI 
          =======================================================================================*/


        public List<sp_GetListUserRole_Result> ListUserRole_Results()
        {
            string spName = "dbo.sp_GetListUserRole";

            List<sp_GetListUserRole_Result> retVal = new List<sp_GetListUserRole_Result>();
            DataTable dtUser = new DataTable();

            try
            {
                //  cmd.Parameters.Add(new SqlParameter("@Username", spParam.Username));
                using (SqlConnection conn = DbConnectionFactory.CreateConnection())
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtUser);
                    }

                    if (dtUser.Rows.Count > 0)
                    {
                        retVal = dtUser.DataTableToList<sp_GetListUserRole_Result>();
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retVal;
        }

        /*=======================================================================================
           2 LISTA PRODOTTI 
         =======================================================================================*/

        public List<sp_GetListProduct_Result> sp_GetListProduct ()
        {
            string spName = "dbo.sp_GetListProduct";

            List<sp_GetListProduct_Result> retVal = new List<sp_GetListProduct_Result>();
            DataTable dtProduct = new DataTable();

            try
            {
                //  cmd.Parameters.Add(new SqlParameter("@Username", spParam.Username));
                using (SqlConnection conn = DbConnectionFactory.CreateConnection())
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtProduct);
                    }

                    if (dtProduct.Rows.Count > 0)
                    {
                        retVal = dtProduct.DataTableToList<sp_GetListProduct_Result>();
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retVal;
        }

        /*=======================================================================================
           3 LISTA CATEGORIEPRODOTTI 
         =======================================================================================*/

        public List<sp_GetListProductCategory_Result> sp_GetListProductCategory ()
        {
            string spName = "dbo.sp_GetListProductCategory";

            List<sp_GetListProductCategory_Result> retVal = new List<sp_GetListProductCategory_Result>();
            DataTable dtCategory = new DataTable();

            try
            {
                //  cmd.Parameters.Add(new SqlParameter("@Username", spParam.Username));
                using (SqlConnection conn = DbConnectionFactory.CreateConnection())
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtCategory);
                    }

                    if (dtCategory.Rows.Count > 0)
                    {
                        retVal = dtCategory.DataTableToList<sp_GetListProductCategory_Result>();
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
