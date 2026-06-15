using BLL_Ecommerce.Interfaaces;
using DAL_Ecommerce;
using DTO_Ecommerce;

namespace BLL_Ecommerce.Services
{
    public class LogService : ILogService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idUser"></param>
        /// <returns></returns>
        /// 
        public int WriteLogLogin(int? idUser)
        {
            sp_ManageLog_Param spParam = new sp_ManageLog_Param();

            spParam.IdUser = idUser;
            spParam.IdLogSeverity = Enums.severityLog.INFO;
            spParam.IdLogType = Enums.typeLog.LOGIN;
            spParam.ErrorMessage = "L'utente ha effettuato il login";

            return this.WriteLog(ref spParam);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idUser"></param>
        /// <returns></returns>
        public int WriteLogLogout(int? idUser)
        {
            sp_ManageLog_Param spParam = new sp_ManageLog_Param();

            spParam.IdUser = idUser;
            spParam.IdLogSeverity = Enums.severityLog.INFO;
            spParam.IdLogType = Enums.typeLog.LOGOUT;
            spParam.ErrorMessage = "L'utente ha effettuato il logout";

            return this.WriteLog(ref spParam);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idUser"></param>
        /// <returns></returns>
        public int WriteLogProcessError(int? idUser, string errorMessage)
        {
            sp_ManageLog_Param spParam = new sp_ManageLog_Param();

            spParam.IdUser = idUser;
            spParam.IdLogSeverity = Enums.severityLog.ERROR;
            spParam.IdLogType = Enums.typeLog.PROCESS;
            spParam.ErrorMessage = errorMessage;

            return this.WriteLog(ref spParam);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idUser"></param>
        /// <returns></returns>
        public int WriteLogProcessWarning(int? idUser, string errorMessage)
        {
            sp_ManageLog_Param spParam = new sp_ManageLog_Param();

            spParam.IdUser = idUser;
            spParam.IdLogSeverity = Enums.severityLog.WARNING;
            spParam.IdLogType = Enums.typeLog.PROCESS;
            spParam.ErrorMessage = errorMessage;

            return this.WriteLog(ref spParam);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idUser"></param>
        /// <returns></returns>
        public int WriteLogProcessInfo(int? idUser)
        {
            sp_ManageLog_Param spParam = new sp_ManageLog_Param();

            spParam.IdUser = idUser;
            spParam.IdLogSeverity = Enums.severityLog.INFO;
            spParam.IdLogType = Enums.typeLog.PROCESS;

            return this.WriteLog(ref spParam);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spParam"></param>
        /// <returns></returns>
        private int WriteLog(ref sp_ManageLog_Param spParam)
        {
            DAL_Log logDal = new DAL_Log();

            return logDal.WriteLog(ref spParam);
        }
    }
}
