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
                throw ex;
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

                    
                  
                    
                        DataTable dtOrder_Header = null;
                        DataTable dtOrder_Detail = null;


                        if(spParam.Mode == "H")  //  H=Header, D=Detail, B=Both
                        {
                            dtOrder_Header = dsList.Tables[0];
                        }
                        else if (spParam.Mode == "D")
                        {
                            dtOrder_Detail = dsList.Tables[0];
                        }
                        else
                        {
                            dtOrder_Header = dsList.Tables[0];
                            dtOrder_Detail = dsList.Tables[1];
                        }

                    //  se dtOrder_Header non è vuoto  Oppure dtOrder_Detail non è vuoto  dtOrder_Header è maggiore di 0
                    //  oppure dtOrder_Detail > 0 fai le istruzioni successive 


                    // se H O B il risultato va nel campo  public sp_GetPriceListByIdHeader_Result Header { get; set; }
                    //sp_GetPriceListByIdHeader_Result = dtOrder_Header

                    // se D O B il risultato va nel campo public List<sp_GetPriceListByIdDetail_Result> Details { get; set; }

                    //  List<sp_GetPriceListByIdDetail_Result> Details = dtOrder_Detail -> lista



                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return retVal;
        }

        public void ManagePriceList(sp_ManagePriceList_Param param)
        {
            string spName = "dbo.sp_ManagePriceList";



            using (SqlConnection conn = DbConnectionFactory.CreateConnection())
            using (SqlCommand cmd = new SqlCommand(spName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Mappatura dei parametri con gestione dei valori NULL
                cmd.Parameters.Add("@Operation", SqlDbType.Char, 1).Value = param.Operation;
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = (object)param.Id ?? DBNull.Value;
                cmd.Parameters.Add("@PriceListName", SqlDbType.NVarChar, 50).Value = (object)param.PriceListName ?? DBNull.Value;
                cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 250).Value = (object)param.Description ?? DBNull.Value;
                cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = (object)param.StartDate ?? DBNull.Value;
                cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = (object)param.EndDate ?? DBNull.Value;
                cmd.Parameters.Add("@CurrencyCode", SqlDbType.NVarChar, 3).Value = (object)param.CurrencyCode ?? DBNull.Value;
                cmd.Parameters.Add("@IsDefault", SqlDbType.Bit).Value = (object)param.IsDefault ?? DBNull.Value;

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

    }
}
