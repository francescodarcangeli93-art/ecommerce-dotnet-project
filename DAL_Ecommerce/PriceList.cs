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

            sp_GetPriceListByIdHeader_Result Header = new sp_GetPriceListByIdHeader_Result();
            List<sp_GetPriceListByIdDetail_Result> Details = new List<sp_GetPriceListByIdDetail_Result>();
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

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsList); // È qui che il DataSet inizia a prendere i dati dal database!
                    }


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

                    
                    if((dtOrder_Header != null && dtOrder_Header.Rows.Count > 0) || (dtOrder_Detail != null && dtOrder_Detail.Rows.Count > 0))
                    {

                        // se H O B il risultato va nel campo  public sp_GetPriceListByIdHeader_Result Header { get; set; }
                        if(spParam.Mode == "H" || spParam.Mode == "B")
                        {
                            Header = dtOrder_Header.Rows[0].ToObject<sp_GetPriceListByIdHeader_Result>();

                            if(Header != null)
                            {
                                retVal.Header = Header;
                            }
                        }


                        // se D O B il risultato va nel campo public List<sp_GetPriceListByIdDetail_Result> Details { get; set; }
                        if (spParam.Mode == "D" || spParam.Mode == "B")
                        {
                            
                            Details = dtOrder_Detail.DataTableToList<sp_GetPriceListByIdDetail_Result>();


                            if (Details != null )
                            {
                                retVal.Details = Details;
                            }
                        }

                       
                    }

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



        public int? ManagePriceListItem(sp_ManagePriceListItem_Param spParam)
        {
            string spName = "dbo.sp_ManagePriceListItem";
            int? newId = null;

            using (SqlConnection conn = DbConnectionFactory.CreateConnection())
            using (SqlCommand cmd = new SqlCommand(spName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@Operation", spParam.Operation));
                cmd.Parameters.Add(new SqlParameter("@Id", (object)spParam.Id ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@PriceListId", (object)spParam.PriceListId ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@ProductId", (object)spParam.ProductId ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@Price", (object)spParam.Price ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@StartDate", (object)spParam.StartDate ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@EndDate", (object)spParam.EndDate ?? DBNull.Value));

                try
                {
                    conn.Open();

                    if (spParam.Operation == "I")
                    {
                        // Insert restituisce un result set (NewId), serve ExecuteScalar
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            newId = Convert.ToInt32(result);
                        }
                    }
                    else
                    {
                        // Update e Delete non restituiscono nulla
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }

            return newId;
        }
    }
}
