using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTODoc.Attributes
{
    public class DbFieldAttribute : Attribute
    {

        private string _fieldName;
        private SqlDbType _dataType;
        private bool _IsPrimaryKey;
        private bool _autoIncrement;
        private AutoIncrementMode _autoIncrementMode;

        /// <summary>
        /// Definisce le modalità di autoincrement previste
        /// </summary>
        public enum AutoIncrementMode
        {
            /// <summary>
            /// Modalità di auto increment manuale basata su trigger e sequence
            /// </summary>
            MANUAL_BY_SEQUENCE = 0,

            /// <summary>
            /// Modalità per la quale Oracle genera sempre un valore per la colonna Identity. Il tentativo di inserire un valore nella colonna Identity causerà un errore.
            /// </summary>
            GENERATED_ALWAYS_AS_IDENTITY = 1,

            /// <summary>
            /// Modalità per la quale Oracle genera un valore per la colonna Identity se non fornisci alcun valore. Se fornisci un valore, Oracle inserirà quel valore nella colonna Identity. Per questa opzione, Oracle emetterà un errore se si inserisce un valore NULL nella colonna Identity.
            /// </summary>
            GENERATED_BY_DEFAULT = 2,

            /// <summary>
            /// Modalità per la quale Oracle genera un valore per la colonna Identity se si fornisce un valore NULL o nessun valore.
            /// </summary>
            GENERATED_BY_DEFAULT_ON_NULL = 3

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="dataType"></param>
        /// <param name="primaryKey"></param>
        /// <param name="autoIncrement"></param>
        /// <param name="autoIncrementMode"></param>
        public DbFieldAttribute(string fieldName, SqlDbType dataType, bool primaryKey, bool autoIncrement, AutoIncrementMode autoIncrementMode)
            : this(fieldName, dataType, primaryKey, autoIncrement)
        {
            _autoIncrementMode = autoIncrementMode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="dataType"></param>   
        /// <param name="primaryKey"></param>
        /// <param name="autoIncrement"></param>/// 
        public DbFieldAttribute(string fieldName, SqlDbType dataType, bool primaryKey, bool autoIncrement)
        {
            _fieldName = fieldName;
            _dataType = dataType;
            _IsPrimaryKey = primaryKey;
            _autoIncrement = autoIncrement;
            _autoIncrementMode = DbFieldAttribute.AutoIncrementMode.MANUAL_BY_SEQUENCE;
        }

        public string FieldName
        {
            get
            {
                return _fieldName;
            }
        }

        public SqlDbType DataType
        {
            get
            {
                return _dataType;
            }
        }

        public bool IsPrimaryKey
        {
            get
            {
                return _IsPrimaryKey;
            }
        }

        public bool AutoIncrement
        {
            get
            {
                return _autoIncrement;
            }
        }

        /// <summary>
        /// Ottiene la modalità di autoincrement specificata nella definizione dell'attributo
        /// </summary>
        public AutoIncrementMode IncrementMode
        {
            get
            {
                return _autoIncrementMode;
            }
        }

    }
}
