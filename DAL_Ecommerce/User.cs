using DTO_Ecommerce;
using DTO_Ecommerce.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_Ecommerce
{
    public class DAL_User
    {
        public List<sp_SearchUser_Result> SearchUser (sp_SearchUser_Param spParam)
        {
            Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();

            string spName = "dbo.sp_SearchUser";

            List<sp_SearchUser_Result> retVal = new List<sp_SearchUser_Result>();  
            DataTable dtUser = new DataTable(); 

            try
            {
                using (SqlConnection conn = DbConnectionFactory.CreateConnection())
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    System.Diagnostics.Debug.WriteLine($"[1] Dopo CreateConnection: {sw.ElapsedMilliseconds}ms");

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Id", (object)spParam.Id ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@RoleId", (object)spParam.RoleId ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@Username", (object)spParam.Username ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@Email", (object)spParam.Email ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@FirstName", (object)spParam.FirstName ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@LastName", (object)spParam.LastName ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@IsActive", (object)spParam.IsActive ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@CustomerId", (object)spParam.CustomerId ?? DBNull.Value));


                    conn.Open();
                    System.Diagnostics.Debug.WriteLine($"[2] Dopo conn.Open(): {sw.ElapsedMilliseconds}ms");

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var user = new sp_SearchUser_Result
                            {
                                // Mappiamo solo i campi REALI che ritornano dal DB
                                Id = reader["Id"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["Id"]),
                                RoleId = reader["RoleId"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["RoleId"]),
                                RoleName = reader["RoleName"].ToString(),
                                Username = reader["Username"].ToString(),
                                Email = reader["Email"].ToString(),
                                CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                                IsActive = Convert.ToBoolean(reader["IsActive"]),
                                Password = reader["Password"].ToString(),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                CustomerId = reader["CustomerId"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["CustomerId"])

                                // IconUserRole NON viene toccata qui, si calcolerà da sola quando serve!
                            };
                            retVal.Add(user);
                        }
                    }


                }
        
            }
            catch 
            {
                throw ;
            }

            return retVal;
        }

        public sp_GetUserById_Result GetUserById (int? id)
        {
            string spName = "dbo.sp_GetUserById";

            sp_GetUserById_Result User = new sp_GetUserById_Result();

            //  List < sp_GetUserById_Result> retVal = new List<sp_GetUserById_Result>();
           //  DataTable dtUser = new DataTable();

            try
            {
                using (SqlConnection conn = DbConnectionFactory.CreateConnection())
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Id", id));
                  

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            User = new sp_GetUserById_Result
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                RoleId      = Convert.ToInt32(reader["RoleId"]),
                                RoleName    = reader["RoleName"].ToString(),
                                Username    = reader["Username"].ToString(),
                                Email       = reader["Email"].ToString(),
                                Password    = reader["Password"].ToString(),
                                CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                                IsActive    = Convert.ToBoolean(reader["IsActive"]),
                                FirstName   = reader["FirstName"].ToString(),
                                LastName    = reader["LastName"].ToString(),
                                City        = reader["City"].ToString(),
                                Country     = reader["Country"].ToString(),
                                Phone       = reader["Phone"].ToString(),
                                CustomerId  = reader["CustomerId"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["CustomerId"])
                            };
                        }
                    }

               
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return User;
        }

        public  void  sp_ManageUser (sp_ManageUser_Param spParam)
        {
            string spName = "dbo.sp_ManageUser";

           
                using (SqlConnection conn = DbConnectionFactory.CreateConnection())
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@Operation", spParam.Operation));
                    cmd.Parameters.Add(new SqlParameter("@Id", spParam.Id));
                    cmd.Parameters.Add(new SqlParameter("@RoleId", spParam.RoleId));
                    cmd.Parameters.Add(new SqlParameter("@Username", spParam.Username));
                    cmd.Parameters.Add(new SqlParameter("@Email", spParam.Email));
                    cmd.Parameters.Add(new SqlParameter("@Password", spParam.Password));
                    cmd.Parameters.Add(new SqlParameter("@CreatedDate", spParam.CreatedDate));
                    cmd.Parameters.Add(new SqlParameter("@IsActive", spParam.IsActive));
                    cmd.Parameters.Add(new SqlParameter("@CustomerId", spParam.CustomerId));
                    try
                    {
                        conn.Open();
                       cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }

                       
                }
        }

        







    }
}
