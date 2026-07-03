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
    public class DAL_PriceList
    {
        public List<sp_SearchPriceList_Result> SearchPriceList(sp_SearchPriceList_Param spParam)
        {
            string spName = "dbo.sp_SearchPriceList";

            List<sp_SearchPriceList_Result> retVal = new List<sp_SearchPriceList_Result>();
            DataTable dtPrice = new DataTable();

            try
            {
                using (SqlConnection conn = DbConnectionFactory.CreateConnection())
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@PriceListName", (object)spParam.PriceListName ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@Description", (object)spParam.Description ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@StartDate", (object)spParam.StartDate ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@EndDate", (object)spParam.EndDate ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@CurrencyCode", (object)spParam.CurrencyCode ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@IsDefault", (object)spParam.IsDefault ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@IsActive", (object)spParam.IsActive ?? DBNull.Value));

                    // Apertura connessione sincrona
                    conn.Open();

                    // Lettura sincrona dei dati tramite reader
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dtPrice.Load(reader);
                    }

                    if (dtPrice.Rows.Count > 0)
                    {
                        retVal = dtPrice.DataTableToList<sp_SearchPriceList_Result>();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return retVal;
        }

        public sp_GetPriceListById_Result GetPriceListById(sp_GetPriceListById_Param spParam)
        {
            string spName = "dbo.sp_GetPriceListById";

             sp_GetPriceListById_Result retVal = new sp_GetPriceListById_Result();

            List<sp_GetPriceListByIdDetail_Result> header = new List<sp_GetPriceListByIdDetail_Result>();

            DataSet dsList = new DataSet();

            try
            {
                using (SqlConnection conn = DbConnectionFactory.CreateConnection())
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@PriceListId", (object)spParam.PriceListId ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@PriceListItemId", (object)spParam.PriceListItemId ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@ProductId", (object)spParam.ProductId ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@Mode", (object)spParam.Mode ?? DBNull.Value));
                   
                    // Apertura connessione sincrona
                    conn.Open();

                    
                  
                    if(dsList.Tables[0].Rows.Count > 0 || dsList.Tables[1].Rows.Count > 0)
                    {
                        DataTable dtOrder_Header = null;
                        DataTable dtOrder_Detail = null;


                        if(spParam.Mode == "H")  //  H=Header, D=Detail, B=Both
                        {
                            dtOrder_Header = dsList.Tables[0];
                        }
                        else if (spParam.Mode == "D")
                        {
                            dtOrder_Detail = dsList.Tables[1];
                        }
                        else
                        {
                            dtOrder_Header = dsList.Tables[0];
                            dtOrder_Detail = dsList.Tables[1];
                        }


                        // se H il risultato va nel campo  public sp_GetPriceListByIdHeader_Result Header { get; set; }

                        // se D il risultato va nel campo public List<sp_GetPriceListByIdDetail_Result> Details { get; set; }


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
