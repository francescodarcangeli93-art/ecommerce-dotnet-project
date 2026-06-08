using DTO_Ecommerce.DataModels;
using System;
using System.Collections.Generic;
using System.Data;

namespace DTO_Ecommerce.Extensions
{
    public static class DataReaderExtension
    {
        /// <summary>
        /// Restituisce la lista di classi
        /// </summary>
        /// <typeparam name="T">Tipologia del dato da restituire</typeparam>
        /// <param name="reader">Istanza della classe DataReader</param>
        /// <returns>La lista con gli elementi</returns>
        public static List<T> ToList<T>(this IDataReader reader) where T : class, IRecord
        {
            var lista = new List<T>();

            var hasSubRecord = typeof(ISubRecord<T>).IsAssignableFrom(typeof(T));
            T prevRecord = null;

            while (reader.Read())
            {
                if (
                     (hasSubRecord & prevRecord != null)
                  && ((ISubRecord<T>)prevRecord).IsSameRecord(prevRecord, reader)
                   )
                {
                    ((ISubRecord<T>)prevRecord).FillSubRecord(prevRecord, reader);
                    continue;
                }

                var i = Activator.CreateInstance<T>();

                i.Fill(reader);
                prevRecord = i;

                if (hasSubRecord)
                    ((ISubRecord<T>)i).FillSubRecord(prevRecord, reader);

                lista.Add(i);
            }

            return lista;
        }

        /// <summary>
        /// Restituisce il valore del campo convertito nel tipo dato impostato
        /// </summary>
        /// <typeparam name="T">Tipologia del dato da restituire</typeparam>
        /// <param name="reader">Istanza della classe DataReader</param>
        /// <param name="fieldName">Nome della colonna per recuperare il valore</param>
        /// <returns>Il valore del campo richiesto</returns>
        public static T GetValue<T>(this IDataReader reader, string fieldName)
        {
            try
            {
                var fieldindex = reader.GetOrdinal(fieldName);
                if (reader.IsDBNull(fieldindex))
                    return default(T);

                var dataType = Nullable.GetUnderlyingType(typeof(T));
                return (T)Convert.ChangeType(reader.GetValue(fieldindex), dataType ?? typeof(T));
            }
            catch (IndexOutOfRangeException ire)
            {
                throw new IndexOutOfRangeException(string.Format("Campo \"{0}\" non trovato nel recordset", fieldName), ire);
            }
        }

        /// <summary>
        /// Verifica la presenza dell campo specificato
        /// </summary>
        /// <param name="r">Istanza della classe DataReader</param>
        /// <param name="columnName">Nome della colonna da cercare</param>
        /// <returns>Valore booleano che individua la presenza</returns>
        public static bool HasColumn(this IDataRecord r, string columnName)
        {
            try
            {
                return r.GetOrdinal(columnName) >= 0;
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
        }

        /// <summary>
        /// Indica se il campo specificato è impostato a NULL
        /// </summary>
        /// <param name="r">Istanza della classe DataReader</param>
        /// <param name="columnName">Nome del campo da verificare</param>
        /// <returns>Valore booleano che indica se uguale a NULL</returns>
        public static bool IsDBNull(this IDataRecord r, string columnName)
        {
            try
            {
                var colidx = r.GetOrdinal(columnName);
                return r.IsDBNull(colidx);
            }
            catch (IndexOutOfRangeException ire)
            {
                throw new IndexOutOfRangeException(string.Format("Campo \"{0}\" non trovato nel recordset", columnName), ire);
            }
        }


    }
}
