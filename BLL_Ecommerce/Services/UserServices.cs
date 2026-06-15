using BLL_Ecommerce.Interfaces;
using DAL_Ecommerce;
using DTO_Ecommerce;
using System;
using System.Collections.Generic;

namespace BLL_Ecommerce.Services
{
    public class UserServices : IUserService
    {
        DAL_User User =  new DAL_User();   

        public List<sp_SearchUser_Result> SearchUser(sp_SearchUser_Param spParam)
        {

            //List<sp_SearchUser_Result> retVal = new List<sp_SearchUser_Result>();   

            //retVal = User.SearchUser(spParam);    

            return User.SearchUser(spParam);
        }


        public sp_GetUserById_Result GetUserById(sp_GetUserById_Param spParam)
        {
            sp_GetUserById_Result retVal = new sp_GetUserById_Result();

            retVal = User.GetUserById(spParam.Id);

            retVal.Password = EncryptionHelper.Decrypt(retVal.Password);

           

            return retVal;
        }

        public sp_GetUserById_Result GetUserById(int? id)
        {
            sp_GetUserById_Result retVal = new sp_GetUserById_Result();

            retVal = User.GetUserById(id);

            retVal.Password = EncryptionHelper.Decrypt(retVal.Password);

            //if (retVal != null)
            //{
            //    retVal.Password = "***********";
            //}

            return retVal;
        }




        public void sp_ManageUser(sp_ManageUser_Param spParam)
        {
            // serve il try and catch dato che la proc ha la transaction?
            spParam.Password = EncryptionHelper.Encrypt(spParam.Password);
            try
            {
                User.sp_ManageUser(spParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

    }
}
