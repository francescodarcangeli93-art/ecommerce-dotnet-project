using System.ComponentModel;

namespace DTO_Ecommerce
{
    public class Enums
    {
        public enum severityLog
        {
            [Description("#Severity Info")]
            INFO   = 1,
            [Description("#Severity Warning")]
            WARNING = 2,
            [Description("#Severity Error")]
            ERROR  = 3
        }

        public enum typeLog
        {
            [Description("#Type Login")]
            LOGIN  = 1,
            [Description("#Type Logout")]
            LOGOUT = 2,
            [Description("#Type Process")]
            PROCESS  = 3
        }

        //public enum typeModeView
        //{
        //    [Description("#Visualizzazione")]
        //    V = 1,
        //    [Description("#Inserimento")]
        //    I = 2,
        //    [Description("#Modifica")]
        //    M = 3
        //}
    }
}
