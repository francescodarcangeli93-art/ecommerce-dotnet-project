using DTO_Ecommerce;
using DTO_Ecommerce.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL_Ecommerce
{
    public class DAL_Order
    {

        public List<sp_GetOrderById_Result> GetOrderById(sp_GetOrderById_Param param)
        {
            List<sp_GetOrderById_Result> retVal = new List<sp_GetOrderById_Result>();

            sp_GetOrderByIdHeader_Result retValHeader = new sp_GetOrderByIdHeader_Result();
            List<sp_GetOrderByIdDetail_Result> retValDetail = new List<sp_GetOrderByIdDetail_Result>();

            string spName = "dbo.sp_GetOrderById";

            DataSet dsOrder = new DataSet();

            try
            {
                using (SqlConnection conn = DbConnectionFactory.CreateConnection())
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@OrderId", param.OrderId));
                    cmd.Parameters.Add(new SqlParameter("@OrderItemId", param.OrderItemId));
                    cmd.Parameters.Add(new SqlParameter("@Mode", param.Mode));

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsOrder);
                    }

                    DataTable dtOrder_Header = null;
                    DataTable dtOrder_Detail = null;

                    //Riempio i DataTables a seconda dei casi
                    switch (param.Mode)
                    {
                        case "H":
                            {
                                dtOrder_Header = dsOrder.Tables[0];
                                break;
                            }
                        case "D":
                            {
                                dtOrder_Detail = dsOrder.Tables[0];
                                break;
                            }
                        case "B":
                            {
                                dtOrder_Header = dsOrder.Tables[0];
                                dtOrder_Detail = dsOrder.Tables[1];
                                break;
                            }
                    }

                    //Riempio il retVal a seconda dei casi
                    if (param.Mode == "H" || param.Mode == "B")
                    {
                        retValHeader = dtOrder_Header.Rows[0].ToObject<sp_GetOrderByIdHeader_Result>();

                        if (retValHeader != null)
                        {
                            retVal[0].OrderHeader = retValHeader;
                        }
                    }

                    if (param.Mode == "D" || param.Mode == "B")
                    {
                        retValDetail = dtOrder_Detail.DataTableToList<sp_GetOrderByIdDetail_Result>();

                        if (retValDetail != null && retValDetail.Count > 0)
                        {
                            retVal[0].OrderDetails = retValDetail;
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


    }
}
